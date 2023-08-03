using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data
{
    public  interface ICoffeeCountStore
    {
        void Save(CoffeeCountItem item);
    }
}
