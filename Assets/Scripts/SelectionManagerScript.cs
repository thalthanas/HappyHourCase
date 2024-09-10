using System.Collections.Generic;

public class SelectionManagerScript
{
    private static SelectionManagerScript _instance;

    public static SelectionManagerScript Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new SelectionManagerScript();
            }

            return _instance;
        }

        private set
        {
            _instance = value;
        }

    }

    public HashSet<SelectableUnitScript> SelectedUnits = new HashSet<SelectableUnitScript>();
    public List<SelectableUnitScript> AvailableUnits = new List<SelectableUnitScript>();


    private SelectionManagerScript() {}

    public void Select(SelectableUnitScript Unit)
    {
        if(Unit.isLocal)
        {
            SelectedUnits.Add(Unit);
            Unit.OnSelected();
        }
       
    }

    public void DeSelect(SelectableUnitScript Unit)
    {
        Unit.OnDeselect();
        SelectedUnits.Remove(Unit);
    }

    public void DeSelectAll()
    {
        foreach (SelectableUnitScript unit in SelectedUnits)
        {
            unit.OnDeselect();
        }
        SelectedUnits.Clear();
    }


    public bool IsSelected(SelectableUnitScript Unit)
    { 
        return SelectedUnits.Contains(Unit); 
    }
}
