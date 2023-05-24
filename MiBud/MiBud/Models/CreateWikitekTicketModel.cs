using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MiBud.Models
{
    public class CreateWikitekTicketModel : BaseViewModel
    {
        private int _id;
        public int id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("id");
            }
        }

        private string _name;
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        private Color _text_color;
        public Color text_color
        {
            get => _text_color;
            set
            {
                _text_color = value;
                OnPropertyChanged("text_color");
            }
        }

        private bool _add_btn_visible;
        public bool add_btn_visible
        {
            get => _add_btn_visible;
            set
            {
                _add_btn_visible = value;
                OnPropertyChanged("add_btn_visible");
            }
        }

        private bool _delete_btn_visible;
        public bool delete_btn_visible
        {
            get => _delete_btn_visible;
            set
            {
                _delete_btn_visible = value;
                OnPropertyChanged("delete_btn_visible");
            }
        }

        private bool _right_btn_visible;
        public bool right_btn_visible
        {
            get => _right_btn_visible;
            set
            {
                _right_btn_visible = value;
                OnPropertyChanged("right_btn_visible");
            }
        }

        private bool _wrong_btn_visible;
        public bool wrong_btn_visible
        {
            get => _wrong_btn_visible;
            set
            {
                _wrong_btn_visible = value;
                OnPropertyChanged("wrong_btn_visible");
            }
        }

        private bool _drop_down_visible;
        public bool drop_down_visible
        {
            get => _drop_down_visible;
            set
            {
                _drop_down_visible = value;
                OnPropertyChanged("drop_down_visible");
            }
        }
        private bool _line_visible;
        public bool line_visible
        {
            get => _line_visible;
            set
            {
                _line_visible = value;
                OnPropertyChanged("line_visible");
            }
        }
    }
}
