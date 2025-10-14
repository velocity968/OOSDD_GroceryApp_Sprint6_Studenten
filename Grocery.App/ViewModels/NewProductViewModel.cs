using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private int stock;

        [ObservableProperty]
        private DateOnly shelfLife = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public NewProductViewModel(IProductService productService)
        {
            _productService = productService;
        }

        [RelayCommand]
        private async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Productnaam is verplicht";
                return;
            }

            if (Price <= 0)
            {
                ErrorMessage = "Prijs moet groter zijn dan 0";
                return;
            }

            if (Stock < 0)
            {
                ErrorMessage = "Voorraad kan niet negatief zijn";
                return;
            }

            try
            {
                var newProduct = new Product(0, Name, Stock, ShelfLife, Price);

                _productService.Add(newProduct);

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij opslaan: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}