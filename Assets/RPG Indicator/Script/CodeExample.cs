using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG_Indicator;

public class CodeExample : MonoBehaviour
{
    public RpgIndicator PlayerIndicator;

    // Note
    // ShowRangeIndicator will activate the range indicator before casting
    // RpgIndicator.IndicatorAlignement.Ally will determine the color to use when showing the indicator
    // Style refer to the array od RPGIndicatorData. It will affect the colors, materials and layer to use
    public delegate void OnChangeSkillType();
    public event OnChangeSkillType onChangeSkillType;
    private void Start()
    {
        //  Radius();

    }
    public void Cone(float angle,float range)
    {
        // Cone ability with a 40 degree angle and range of 10
        if (onChangeSkillType != null) onChangeSkillType();
        PlayerIndicator.ShowCone(angle, range, true, RpgIndicator.IndicatorColor.Ally, 0);
    }
    public void Line(float length,float range)
    {
        if (onChangeSkillType != null) onChangeSkillType();
        // Line ability with a length og 6 and range of 10
        PlayerIndicator.ShowLine(length, range, true, RpgIndicator.IndicatorColor.Ally, 0);
    }
    public void Area(float radius,float range)
    {
        if (onChangeSkillType != null) onChangeSkillType();
        // Area ability with a radius of 5 and range of 10 and with 2 custom colors
        PlayerIndicator.CustomColor("#80989700", "#80989700");
        PlayerIndicator.ShowArea(radius, range, true, RpgIndicator.IndicatorColor.Custom, 0);
    }
    public void Radius(float radius)
    {
        if (onChangeSkillType != null) onChangeSkillType();
        // Radius ability with a radius of 10
        PlayerIndicator.ShowRadius(radius, false, RpgIndicator.IndicatorColor.Enemy, 0);
    }
    public void Cast(float time)
    {
        // Start casting with a casting time of 5 seconds
        PlayerIndicator.Casting(time);
    }
    public void Interrupt()
    {
        // Interrupt casting
        PlayerIndicator.InterruptCasting();
    }
}
