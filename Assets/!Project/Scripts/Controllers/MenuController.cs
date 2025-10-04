using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hitsCountText;
    [SerializeField] private Toggle RandomMassRadiusToggle;
    [SerializeField] private Toggle EnableCutToggle;
    [SerializeField] private Slider MuzzleAngleSlider;
    [SerializeField] private Slider DragCoefficientSlider;
    [SerializeField] private Slider AirDensitySlider;
    [SerializeField] private Button WindButton;
    [SerializeField] private TraectoryRenderer traectoryRenderer;

    private void Update() => hitsCountText.text = $"Количество попаданий: {TargetSpawner.instance.hitsCount}";

    public void OnRandomMassRadiusToggleChange() => BalisticCalculator.instance.isRandomMassRadius = RandomMassRadiusToggle.isOn;

    public void OnEnableCutToggleChange() => traectoryRenderer.enableTraectoryCut = EnableCutToggle.isOn;

    public void OnMuzzleAngleSliderChange() => BalisticCalculator.instance.muzzleAngle = MuzzleAngleSlider.value;

    public void OnDragCoefficientSliderChange() => BalisticCalculator.instance.dragCoefficient = DragCoefficientSlider.value;

    public void OnAirDensitySliderChange() => BalisticCalculator.instance.airDensity = AirDensitySlider.value;

    public void OnWindButtonClick() =>
        BalisticCalculator.instance.wind = new Vector3(
            Convert.ToInt32(CleanStringBeforeToInt(WindButton.transform.parent.GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text)),
            Convert.ToInt32(CleanStringBeforeToInt(WindButton.transform.parent.GetChild(1).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text)),
            Convert.ToInt32(CleanStringBeforeToInt(WindButton.transform.parent.GetChild(2).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text))
            );

    /// <summary>
    /// Prevents an exception due to an extra unicode character due to the InputField. Returns a normal string without any extra unicode characters
    /// </summary>
    private string CleanStringBeforeToInt(string input)
    {
        if (string.IsNullOrEmpty(input)) return "0";
        return new string(input.Where(c => char.IsDigit(c) || c == '-').ToArray());
    }
}