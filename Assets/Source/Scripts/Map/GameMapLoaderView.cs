using UnityEngine;

public class GameMapLoaderView : MonoBehaviour, IPresenterView
{
    [field: SerializeField] public Transform MapStartPositionTransform { get; private set; }
}