namespace NetGore.EditorTools.Docking
{
    interface IContentFocusManager
    {
        void Activate(IDockContent content);

        void AddToList(IDockContent content);

        void GiveUpFocus(IDockContent content);

        void RemoveFromList(IDockContent content);
    }
}