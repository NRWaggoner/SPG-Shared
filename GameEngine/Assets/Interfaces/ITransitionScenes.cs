using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    public interface ICoordinateTransitions
    {
        void SwapInNextScene();
        void StartNextScene();
    }

    public interface ITransitionScenes
    {
        void StartTransition();
        void Initialize(float horizontalSize, float verticalSize, ICoordinateTransitions coordinator);

        void Hide();

    }
}
