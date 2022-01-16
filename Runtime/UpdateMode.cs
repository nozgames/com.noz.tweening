namespace NoZ.Tweenz
{
    public enum UpdateMode : byte
    {
        /// <summary>
        /// Execute on the default Update 
        /// </summary>
        Default,

        /// <summary>
        /// Execute on the LateUpdate
        /// </summary>
        Late,

        /// <summary>
        /// Execute on the FixedUpdate
        /// </summary>
        Fixed,

        /// <summary>
        /// Execute only with the Evaluate method
        /// </summary>
        Manual
    }
}