public enum NecessityDegree {
    /// <summary>
    /// Will be implemented during the generation if it's possible
    /// </summary>
    Desirable = 1,

    /// <summary>
    /// There will be throwed an exception if the required won't be
    /// implemented during generation
    /// </summary>
    Required = 2
}
