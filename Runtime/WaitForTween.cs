using UnityEngine;

namespace NoZ.Tweenz
{
    /// <summary>
    /// Waits for a tween to finish
    /// </summary>
    public sealed class WaitForTween : CustomYieldInstruction
    {
        private Tween _tween;

        /// <summary>
        /// Constructs a yield instruction that waits for the <paramref name="tween"/> to finish.
        /// </summary>
        /// <param name="tween">Tween to wait for</param>
        public WaitForTween(Tween tween) => _tween = tween;

        /// <summary>
        /// Keep waiting until the tween is done
        /// </summary>
        public override bool keepWaiting => !_tween.isDone;
    }
}

