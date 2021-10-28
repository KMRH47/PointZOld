namespace PointZ.Models.CustomEditor
{
    public class CustomEditorEventArgs
    {
        public CustomEditorEventArgs(TextInputTypes textInputTypes) => TextInputTypes = textInputTypes;

        public TextInputTypes TextInputTypes { get;  }
    }
}