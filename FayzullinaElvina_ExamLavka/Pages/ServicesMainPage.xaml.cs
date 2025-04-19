using FayzullinaElvina_ExamLavka.DBConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FayzullinaElvina_ExamLavka.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesMainPage.xaml
    /// </summary>
    public partial class ServicesMainPage : Page
    {
        public static List<Workers> workers { get; set; }
        Workers contextWorker;
        private int pageNumber = 1;
        private int pageCount = 1;
        private List<Services> allServices = new List<Services>();
        public ServicesMainPage(Workers worker)
        {
            InitializeComponent();
            workers = new List<Workers>(DBConnect.DB.Workers.ToList());
            contextWorker = worker;
            var filter = DBConnect.DB.CollectionService.ToList();
            filter.Insert(0, new CollectionService { Name = "Все коллекции" });
            CollectionCB.ItemsSource = filter;
            if(contextWorker.IdRole == 2)
            {
                AddServiceBT.Visibility = Visibility.Visible;
            }
            else
            {
                AddServiceBT.Visibility = Visibility.Collapsed;
            }
                Refresh();
        }

        private void Refresh()
        {
            LVServices.SelectionMode = SelectionMode.Multiple;
            
            allServices = DBConnect.DB.Services.ToList();
           
            // Создаем список продуктов
            var serviceList = allServices.Select(p => new
            {
                ImageService = p.Image,
                Title = p.Name,
                Description = p.Description ?? "Описание отсутствует",
                ProductTypeTitle = p.TypeService?.Name ?? "Неизвестный тип",
                CollectionName = p.CollectionService?.Name ?? "Неизвестная коллекция",
                Cost = p.Cost,
                Collection = p.CollectionService,
                Type = p.TypeService,
                
            }).ToList();

            // Поиск
            var searchText = SearchTB.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                serviceList = serviceList.Where(x =>
                    x.Title.ToLower().Contains(searchText) ||
                    (x.Description?.ToLower().Contains(searchText) ?? false)
                ).ToList();
            }

            // Фильтрация
            var typeFilter = CollectionCB.SelectedItem as CollectionService;
            if (typeFilter != null && CollectionCB.SelectedIndex != 0)
            {
                serviceList = serviceList.Where(x => x.Collection == typeFilter).ToList();
            }

            

            // Пересчитываем количество страниц после фильтрации и сортировки
            pageCount = serviceList.Count % 4 > 0 ? serviceList.Count / 4 + 1 : serviceList.Count / 4;

            // Корректируем номер страницы, если он выходит за границы
            if (pageNumber > pageCount)
                pageNumber = pageCount;
            if (pageNumber < 1)
                pageNumber = 1;

            // Применяем постраничный вывод только после всех фильтров и сортировок
            serviceList = serviceList.Skip((pageNumber - 1) * 4).Take(4).ToList();


            NavSp.Children.Clear(); // Очищаем старые кнопки

            if (pageCount > 1)
            {
                Button button1 = new Button
                {
                    Content = "<",
                    IsHitTestVisible = pageNumber > 1,
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderBrush = new SolidColorBrush(Colors.Transparent),
                };
                button1.Click += PageBtn_Click;
                NavSp.Children.Add(button1);

                for (int i = 1; i <= pageCount; i++)
                {
                    TextBlock textBlock = new TextBlock()
                    {
                        Text = i.ToString(),
                    };
                    if (i == pageNumber)
                    {
                        textBlock.TextDecorations = TextDecorations.Underline;
                    }
                    Button button2 = new Button
                    {
                        Content = textBlock,
                        IsHitTestVisible = i != pageNumber,
                        Background = new SolidColorBrush(Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Colors.Transparent)
                    };
                    button2.Click += PageBtn_Click;
                    NavSp.Children.Add(button2);
                }

                Button button3 = new Button
                {
                    Content = ">",
                    IsHitTestVisible = pageNumber < pageCount,
                    Background = new SolidColorBrush(Colors.Transparent),
                    BorderBrush = new SolidColorBrush(Colors.Transparent)
                };
                button3.Click += PageBtn_Click;
                NavSp.Children.Add(button3);
            }

            LVServices.ItemsSource = serviceList;
        }

        private void PageBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Content.ToString())
            {
                case "<":
                    pageNumber--;
                    break;
                case ">":
                    pageNumber++;
                    break;
                default:
                    pageNumber = int.Parse(((TextBlock)button.Content).Text);
                    break;
            }
            Refresh();
        }
        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            int prevPage = pageNumber; // Запоминаем текущую страницу
            Refresh();

            // Если после обновления есть столько страниц, сколько было, возвращаемся на старую страницу
            if (prevPage <= pageCount)
                pageNumber = prevPage;
            else
                pageNumber = pageCount; // Если страниц стало меньше, переходим на последнюю возможную

            Refresh();
        }

        private void CollectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int prevPage = pageNumber; // Запоминаем текущую страницу
            Refresh();

            // Если после обновления есть столько страниц, сколько было, возвращаемся на старую страницу
            if (prevPage <= pageCount)
                pageNumber = prevPage;
            else
                pageNumber = pageCount; // Если страниц стало меньше, переходим на последнюю возможную

            Refresh();
        }

        private void ExitBT_Click(object sender, RoutedEventArgs e)
        {
            DBConnect.loginedWorker = null;
            NavigationService.Navigate(new AuthorizationPage());
        }

        private void CustomBT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CosplayBT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void AddServiceBT_Click(object sender, RoutedEventArgs e)
        {
            AddServiceWindow addProductWindow = new AddServiceWindow(new Services());
            addProductWindow.ShowDialog(); 
            Refresh();
        }

        private void LVServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = LVServices.SelectedItem;

            if (selectedItem != null)
            {
                // Определяем id выбранного продукта
                var selectedProduct = allServices.FirstOrDefault(p => p.Name == (string)selectedItem.GetType().GetProperty("Title")?.GetValue(selectedItem));

                if (selectedProduct != null)
                {
                    // Открываем окно AddProductWindow и передаем в него выбранный продукт
                    AddServiceWindow addProductWindow = new AddServiceWindow(selectedProduct);
                    addProductWindow.ShowDialog(); // Открываем модально


                    // После закрытия обновляем список (если, например, могли измениться данные)
                    Refresh();
                }
            }
        }
    }
}
