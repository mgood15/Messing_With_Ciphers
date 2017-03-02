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

namespace MessingWithCiphers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            populateComboBox();
            hideEncode();
            hideDecode();
            UpdateLayout();
        }

        // Populates the combobox with all the possible "a" values for the affine cipher.
        private void populateComboBox()
        {
            // the "a" value has to have an inverse to create a unique cipher
            // Therefore in the case of 26, "a" can only be the odd numbers excluding 13
            aValue.Items.Add("1");
            aValue.Items.Add("3");
            aValue.Items.Add("5");
            aValue.Items.Add("7");
            aValue.Items.Add("9");
            aValue.Items.Add("11");
            aValue.Items.Add("15");
            aValue.Items.Add("17");
            aValue.Items.Add("19");
            aValue.Items.Add("21");
            aValue.Items.Add("23");
            aValue.Items.Add("25");
        }
        // hides the decode objects in the GUI
        private void hideDecode()
        {
            decodeButton.Visibility = System.Windows.Visibility.Collapsed;
            decodePlainText.Visibility = System.Windows.Visibility.Collapsed;
            decodeCipherText.Visibility = System.Windows.Visibility.Collapsed;
        }

        // hides the encode objects in the GUI
        private void hideEncode()
        {
            encodeButton.Visibility = System.Windows.Visibility.Collapsed;
            encodePlainText.Visibility = System.Windows.Visibility.Collapsed;
            encodeCipherText.Visibility = System.Windows.Visibility.Collapsed;
            shiftAmount.Visibility = System.Windows.Visibility.Collapsed;
            shiftLabel.Visibility = System.Windows.Visibility.Collapsed;
            aValue.Visibility = System.Windows.Visibility.Collapsed;
            aValueLabel.Visibility = System.Windows.Visibility.Collapsed;
            bValue.Visibility = System.Windows.Visibility.Collapsed;
            bValueLabel.Visibility = System.Windows.Visibility.Collapsed;
        }

        // makes caesar-specific items visible
        private void makeCaesarVisible()
        {
            shiftAmount.Visibility = System.Windows.Visibility.Visible;
            shiftLabel.Visibility = System.Windows.Visibility.Visible;
        }

        // makes affine-specific parameters visible
        private void makeAffineVisible()
        {
            aValue.Visibility = System.Windows.Visibility.Visible;
            aValueLabel.Visibility = System.Windows.Visibility.Visible;
            bValue.Visibility = System.Windows.Visibility.Visible;
            bValueLabel.Visibility = System.Windows.Visibility.Visible;
        }

        // makes encode visible
        private void makeEncodeVisible()
        {
            encodeButton.Visibility = System.Windows.Visibility.Visible;
            encodePlainText.Visibility = System.Windows.Visibility.Visible;
            encodeCipherText.Visibility = System.Windows.Visibility.Visible;
            hideDecode();
            if(affine.IsChecked == true)
            {
                makeAffineVisible();
            }

        }

        // Makes items involving decode visible 
        private void makeDecodeVisible()
        {
            decodeButton.Visibility = System.Windows.Visibility.Visible;
            decodePlainText.Visibility = System.Windows.Visibility.Visible;
            decodeCipherText.Visibility = System.Windows.Visibility.Visible;
            hideEncode();
        }

        // refreshes the state whenever a radio button is triggered
        private void refreshState(object sender, RoutedEventArgs e)
        {
            // check if just encode has been checked
            if(encode.IsChecked == true)
            {
                hideDecode();
                makeEncodeVisible();
            }
            // check if just decode has been checked
            if(decode.IsChecked == true)
            {
                hideEncode();
                makeDecodeVisible();
            }
            // check if encode and caesar has been checked
            if(encode.IsChecked == true && caesar.IsChecked == true)
            {
                hideDecode();
                hideEncode();
                makeEncodeVisible();
                makeCaesarVisible();
            }
            // check if decode and caesar has been checked
            if(decode.IsChecked == true && caesar.IsChecked == true)
            {
                hideEncode();
                makeDecodeVisible();
            }
            // check if encode and affine has been checked
            if(encode.IsChecked == true && affine.IsChecked == true)
            {
                hideDecode();
                hideEncode();
                makeEncodeVisible();
                makeAffineVisible();
            }
            // check if decode and affine has been checked
            if(decode.IsChecked == true && affine.IsChecked == true)
            {
                hideEncode();
                makeDecodeVisible();
            }
        }

        // performs an encoding of caesar or affine cipher
        public void performEncode(object sender, RoutedEventArgs e)
        {
            if(caesar.IsChecked == true)
            {
                String PlainText = encodePlainText.Text;
                String shift = shiftAmount.Text;
                int shiftNumber = 0;
                try
                {
                    shiftNumber = Convert.ToInt32(shift);
                }
                catch (FormatException ex)
                {
                    encodeCipherText.Text = "Error.";
                    return;
                }
                String result = Caesar.encode(PlainText, shiftNumber);
                encodeCipherText.Text = result;
            }
            else if(affine.IsChecked == true)
            {
                String plainText = encodePlainText.Text;
                String a = aValue.Text;
                String b = bValue.Text;

                int aNum = 0;
                try
                {
                    aNum = Convert.ToInt32(a);
                }
                catch (FormatException ex)
                {
                    encodeCipherText.Text = "Error.";
                    return;
                }

                int bNum = 0;
                try
                {
                    bNum = Convert.ToInt32(b);
                }
                catch (FormatException ex)
                {
                    encodeCipherText.Text = "Error.";
                    return;
                }

                String result = Affine.encode(aNum, bNum, plainText);
                encodeCipherText.Text = result;
            }
        }

        // performs decoding of caesar or affine cipher
        public void performDecode(object sender, RoutedEventArgs e)
        {
            if(caesar.IsChecked == true)
            {
                String cipherText = decodeCipherText.Text;
                String result = Caesar.decode(cipherText);
                decodePlainText.Text = result;
            }
            else if(affine.IsChecked == true)
            {
                String cipherText = decodeCipherText.Text;
                String result = Affine.decode(cipherText);
                decodePlainText.Text = result;

            }
            else
            {

            }
        }

    }
}