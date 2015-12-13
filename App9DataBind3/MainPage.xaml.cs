using App9DataBind3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App9DataBind3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // declares the list of dogs
        List<dogClass> _myList;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            //e.GetPosition().X;


            base.OnTapped(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            // get a pointer to the current app instance.
            // similar to fptr = fopen (file)

            App myApp = (Application.Current) as App;
            tblTitle.Text = myApp.strTitle;

            // if the list is not empty, don't fill again
            if (_myList == null)
            {
                // instantiate the list
                _myList = new List<dogClass>();
            }
            else
            {
                _myList.Clear();
            }

            // load the data from a local source
            loadLocalDataAsync();
            //tblTitle.Text = _myList.Count().ToString() + " Dog Breeds";
            // now set as the source for a list view.
            lvDogs.ItemsSource = _myList;
            //lvDogs.ItemsSource = myApp._gListOfdogs;
        }

        #region Move to App.xaml.cs to make the list global.
        private async void loadLocalDataAsync()
        {
            // get the text file (fopen) Data\\myDogs.txt
            var dogFile =
                await Package.Current.InstalledLocation.GetFileAsync("Data\\myDogs.txt");
            // read the text (while(!EOF)
            var fileContent = await FileIO.ReadTextAsync(dogFile);
            // create an array of json objects (generic) JSON.PARSE
            var dogsJsonArray = JsonArray.Parse(fileContent);
            // map these to my list of dogs (have to write this)
            createListOfDogs(dogsJsonArray);
        }

        private void createListOfDogs(JsonArray dogsJsonArray)
        {
            // use the foreach statement to get all the objects
            // from the json array and map to dog objects.
            // json objects are a set of key/value pairs

            foreach (var item in dogsJsonArray)
            {
                // create my object to map the item to.
                dogClass dog = new dogClass();
                // get the object
                var nextObject = item.GetObject();

                // json key/value pairs need to be parse and matched
                foreach (var key in nextObject.Keys)
                {
                    IJsonValue value;   // hold the key value
                    // get the value from the key
                    // value = nextObject.CurrentKey.Value
                    if (!nextObject.TryGetValue(key, out value))
                        continue;

                    // have the value, use a switch to determine
                    // the attribute in the class
                    switch (key) // switch on the name of the key
                    {
                        case "breed":
                            dog.breed = value.ToString();
                            break;
                        case "origin":
                            dog.origin = value.ToString();
                            break;
                        case "grooming":
                            dog.grooming = value.ToString();
                            break;
                        case "activity":
                            dog.exercise = value.ToString();
                            break;
                        case "category":
                            dog.category = value.ToString();
                            break;
                        case "image":
                            dog.imgSource = value.ToString();
                            break;
                    }
                } // end foreach (var key in nextObject.Keys)

                // have complete new dog object
                // add to the list
                _myList.Add(dog);

            } // end foreach (var item in dogsJsonArray)

        }

        #endregion

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(myNextPage));
        }
    }
}
