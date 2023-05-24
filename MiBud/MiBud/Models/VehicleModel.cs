using MiBud.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MiBud.Models
{
    public class VehicleModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public string error { get; set; }
        public bool success { get; set; }
        public HttpStatusCode status_code { get; set; }
        public List<VehicleModelResult> results { get; set; }
    }

    public class VehicleModelResult : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model_file { get; set; }
        public List<VehicleSubModel> sub_models { get; set; }

        private double _selection_item_height = 0;
        public double selection_item_height
        {
            get => _selection_item_height;
            set
            {
                _selection_item_height = value;
                OnPropertyChanged("selection_item_height");
            }
        }

        private double _space_height = 0;
        public double space_height
        {
            get => _space_height;
            set
            {
                _space_height = value;
                OnPropertyChanged("space_height");
            }
        }

        private double _button_height = 0;
        public double button_height
        {
            get => _button_height;
            set
            {
                _button_height = value;
                OnPropertyChanged("button_height");
            }
        }

        private string _arrow = "\U000F054F";
        public string arrow
        {
            get => _arrow;
            set
            {
                _arrow = value;
                OnPropertyChanged("arrow");
            }
        }

        public VehicleOem oem { get; set; }
        public Attachment attachment { get; set; }


        public VehicleSubModel _selected_sub_model = new VehicleSubModel();
        public VehicleSubModel selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;
                OnPropertyChanged("selected_sub_model");
            }
        }

        public VehicleSubModel _selected_model_year = new VehicleSubModel();
        public VehicleSubModel selected_model_year
        {
            get => _selected_model_year;
            set
            {
                _selected_model_year = value;
                OnPropertyChanged("selected_model_year");
            }
        }
    }

    public class VehicleOem
    {
        public int id { get; set; }
        public string name { get; set; }
        public Attachment attachment { get; set; }
    }

    public class Attachment
    {
        public string media_type { get; set; }
        public string attachment { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
    }

    public class VehicleSubModel : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model_year { get; set; }
        public string injector_charecterization { get; set; }
        public string engine_sl_no { get; set; }
        public int segment_name { get; set; }
        public List<VehicleEcu> ecus { get; set; }

        public Attachment attachment { get; set; }


        public bool _selected_sub_model = false;
        public bool selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;
                OnPropertyChanged("selected_sub_model");
            }
        }

    }

    public class VehicleEcu
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tx_header { get; set; }
        public string rx_header { get; set; }
        public List<int> datasets { get; set; }
        public List<int> pid_datasets { get; set; }
        public string protocol { get; set; }
        public string read_dtc_fn_index { get; set; }
        public string clear_dtc_fn_index { get; set; }
        public string read_data_fn_index { get; set; }
        //public WriteDataFnIndex write_data_fn_index { get; set; }
        //public SeedkeyalgoFnIndex seedkeyalgo_fn_index { get; set; }
        //public List<VehicleEcu2> ecu { get; set; } = new List<VehicleEcu2>();
    }

    public class VehicleEcu2
    {
        public int id { get; set; }
        public string sequence_file_name { get; set; }
        public string flashsep_time { get; set; }
        public string flash_address_data_format { get; set; }
        public string flash_check_sum_type { get; set; }
        public string flash_diagnostic_mode { get; set; }
        public string flash_erase_type { get; set; }
        public string flash_frase_byte { get; set; }
        public string flash_nax_blkseqcntr { get; set; }
        public string flash_seed_key_length { get; set; }
        public string flash_status { get; set; }
        public string sequence_file { get; set; }
        public List<File> file { get; set; }

        public string sectorframetransferlen { get; set; }
        public string sendseedbyte { get; set; }
        public List<EcuMapFile> ecu_map_file { get; set; }
    }


    public class Dataset
    {
        public int id { get; set; }
        public string code { get; set; }
    }

    public class PidDataset
    {
        public int id { get; set; }
        public string code { get; set; }
    }

    public class Protocol
    {
        public string name { get; set; }
        public string elm { get; set; }
        public string autopeepal { get; set; }
    }

    public class ReadDtcFnIndex
    {
        public string value { get; set; }
    }

    public class ClearDtcFnIndex
    {
        public string value { get; set; }
    }

    public class ReadDataFnIndex
    {
        public string value { get; set; }
    }

    public class WriteDataFnIndex
    {
        public string value { get; set; }
    }

    public class SeedkeyalgoFnIndex
    {
        public string value { get; set; }
    }

    public class File
    {
        public int id { get; set; }
        public string data_file_name { get; set; }
        public string data_file { get; set; }
    }

    public class EcuMapFile
    {
        public int id { get; set; }
        public string end_address { get; set; }
        public string sector_name { get; set; }
        public string start_address { get; set; }
    }

}
