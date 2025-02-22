﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Core.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Core.Daos;
public class EfCoreDao<T> : IDao<T> where T : class
{
    private readonly ApplicationDbContext _context;

    public EfCoreDao(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task DeleteById(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<object> Insert(T entity)
    {
        try
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

            // Get the primary key property name
            var Pkey = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey();
            if (Pkey.Properties.Count > 1)
            {
                return null;
            }
            var keyname = Pkey.Properties.Select(x => x.Name).Single();

            // Get the value of the primary key
            var keyProperty = typeof(T).GetProperty(keyname);
            if (keyProperty != null)
            {
                return keyProperty.GetValue(entity);
            }

            throw new InvalidOperationException("Entity does not have a primary key property.");
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE constraint failed"))
            {
                // Log the error or handle it as needed
                Console.WriteLine("A record with the same UserId already exists.");
            }
            else
            {
                // Log the error or handle it as needed
                Console.WriteLine($"An error occurred while saving the entity changes: {ex.InnerException?.Message}");
            }
            // Return null or a default value to indicate failure
            return null;
        }
        catch (Exception ex)
        {
            // Log the error or handle it as needed
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            // Return null or a default value to indicate failure
            return null;
        }
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
