using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessor
    {
        private readonly Dictionary<string, int> _countPerCoffeeType = new();
        private readonly ICoffeeCountStore _coffeeCountStore;
        private MachineDataItem? _previouseItem;

        public MachineDataProcessor(ICoffeeCountStore coffeeCountStore)
        {
            _coffeeCountStore = coffeeCountStore;
        }

        public void ProcessItems(MachineDataItem[] dataItems)
        {
            _previouseItem = null; 
            _countPerCoffeeType.Clear();

            foreach (var dataItem in dataItems)
            {
                ProcessItem(dataItem);
            }

            SaveCountPerCoffeeType();
        }

        private void ProcessItem(MachineDataItem dataItem)
        {

            if (!IsNewerThanPreviouseItem(dataItem))
            {
                return;
            }
                if (!_countPerCoffeeType.ContainsKey(dataItem.CoffeeType))
                {
                    _countPerCoffeeType.Add(dataItem.CoffeeType, 1);
                }
                else
                {
                    _countPerCoffeeType[dataItem.CoffeeType]++;
                }

                _previouseItem = dataItem;
        }

        private bool IsNewerThanPreviouseItem(MachineDataItem dataItem)
        {
            return _previouseItem is null
                || _previouseItem.CreatedAt < dataItem.CreatedAt;
        }

        private void SaveCountPerCoffeeType()
        {
            foreach (var entry in _countPerCoffeeType)
            {
               _coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
            }
        }
    }
}
