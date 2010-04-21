using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    class ItemValueTracker
    {
        byte _amount;
        string _description;
        GrhIndex _graphicIndex;
        bool _isNull;
        string _name;
        int _value;

        public byte Amount
        {
            get { return _amount; }
        }

        public string Description
        {
            get { return _description; }
        }

        public GrhIndex GraphicIndex
        {
            get { return _graphicIndex; }
        }

        public bool IsNull
        {
            get { return _isNull; }
        }

        public string Name
        {
            get { return _name; }
        }

        public int Value
        {
            get { return _value; }
        }

        public static bool AreValuesEqual(ItemEntity item, ItemValueTracker tracker)
        {
            // Treat a null ItemValueTracker just like if it was a tracker with IsNull set
            if (tracker == null)
            {
                // If both are null, they are considered equal
                return item == null;
            }

            // Do a normal call to IsEqualTo since the tracker isn't null
            return tracker.IsEqualTo(item);
        }

        public bool IsEqualTo(ItemEntity item)
        {
            if (item == null)
            {
                // If the item is null, then check if ours is also null
                return _isNull;
            }
            else
            {
                // If the item isn't null, but ours is, then return false
                if (_isNull)
                    return false;

                // Neither are null, so check that all values are equal
                return (_amount == item.Amount) && (_value == item.Value) && (_graphicIndex == item.GraphicIndex) &&
                       (_name == item.Name) && (_description == item.Description);
            }
        }

        public void SetValues(ItemEntity item)
        {
            // Check if we are setting the value to a null item
            if (item == null)
            {
                _isNull = true;

                // Release the name and description strings so the GC can maybe clean them up
                _name = null;
                _description = null;

                return;
            }

            // Set the new values
            _isNull = false;
            _name = item.Name;
            _description = item.Description;
            _amount = item.Amount;
            _graphicIndex = item.GraphicIndex;
            _value = item.Value;
        }
    }
}