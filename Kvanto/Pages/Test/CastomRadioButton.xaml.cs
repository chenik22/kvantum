﻿using System;
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

namespace Kvanto.Pages.Test
{
    public partial class CastomRadioButton : UserControl
    {
        public CastomRadioButton()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set {SetValue(TitleProperty, value);}
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            ("Title", typeof(string), typeof(CastomRadioButton));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register
            ("IsActive", typeof(bool), typeof(CastomRadioButton));
    }
}
