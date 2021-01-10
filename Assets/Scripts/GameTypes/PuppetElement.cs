namespace GameTypes
{
    /// <summary>
    /// Light > Dark > Earth > Light - basic rps dynamic
    /// </summary>
    public enum PuppetElement
    {
        None,
        Light,
        Dark,
        Earth
    }

    public static partial class Extensions
    {
        public static PuppetElement Trump (this PuppetElement element)
        {
            switch (element)
            {
                case PuppetElement.Light:
                    return PuppetElement.Earth;
                case PuppetElement.Dark:
                    return PuppetElement.Light;
                case PuppetElement.Earth:
                    return PuppetElement.Dark;
                default:
                    return PuppetElement.None;
            }
        }
    }
}