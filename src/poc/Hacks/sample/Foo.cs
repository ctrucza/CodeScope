namespace sample
{
    public class Foo
    {
        public Foo(string name, int age)
        { }

        public int intField;
        public string stringProp { get; set; }
        public Bar barMethod() { return new Bar(); }

        public Foo complexProp
        {
            get
            {
                return new Foo("hi there", 42);
            }
            set
            {
                var b = new Bar();
                var d = value;
            }
        }
    }
}
