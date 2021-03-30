using System.ComponentModel.DataAnnotations;

namespace SSWProject.Models
{
    public enum HeatingType
    {
        Electric,
        Gas,
        [Display(Name = "Heat Pump")]
        HeatPump,
        [Display(Name = "Oil Furnace")]
        OilFurnace,
        [Display(Name = "Wood Furnace")]
        WoodFurnace,
        Geothermal

    }
}