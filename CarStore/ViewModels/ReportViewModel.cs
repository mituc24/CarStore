using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarStore.Core.Contracts.Services;
using CarStore.Core.Models;
using CarStore.Models.AI;
using CarStore.Services.DataAccess;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarStore.ViewModels;
public class ReportViewModel : ObservableObject
{
    private readonly IDao<Auction> auctionDao;
    private readonly IDao<Car> carDao;
    private static string GEMINI_API_KEY = "AIzaSyCsJR_KT5o6VrmTCvhoVK8xpIuv5TIBGN4";
    public GeminiChatbot gemini = new(GEMINI_API_KEY);


    public List<Auction> Auctions { get; set; }

    public ReportViewModel(IDao<Auction> auctionDao, IDao<Car> carDao)
    {
        this.auctionDao = auctionDao;
        this.carDao = carDao;
        Task.Run(async () => Auctions = await auctionDao.GetAllAsync()).Wait();
    }

    public string CreateListInfoOfAuction(List<Auction> auctions)
    {
        var result = "";
        foreach (Auction auction in auctions)
        {
            Task.Run(async () => auction.Car = await carDao.GetByIdAsync(auction.CarId)).Wait();
            var auctionString = $"Auction ID: {auction.AuctionId}, Auction Name: {auction.Name}, Auction Date: {auction.StartDate}, Car Name: {auction.Car.Name}";
            result += auctionString + "\n";
        }
        return result;
    }

    public async Task<string> Report()
    {
        var prompt = "Generate a report:\n" + CreateListInfoOfAuction(Auctions);
        var listHistory = new List<ChatHistory>();
        var response = await gemini.GenerateResponseAsync(prompt, listHistory);
        Console.WriteLine(response);

        return response;
    }
}
