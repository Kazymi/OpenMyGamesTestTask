using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameMapLoaderRunner : MonoBehaviour
{
    [SerializeField] private Transform _mapStartPositionTransform;
    [Inject] private GameMapLoader _gameMapLoader;

    private void Start()
    {
        _gameMapLoader.LoadMap(_mapStartPositionTransform);
    }
}
