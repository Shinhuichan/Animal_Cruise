using System.Collections;
using UnityEngine;

public class KeySettingcontroller : MonoBehaviour
{
    private KeySettingData currentdata = null;
    public IEnumerator Set_current_co(KeySettingData pressdata)
    {
        if (currentdata == null)
            currentdata = pressdata;
        else
        {
            if (currentdata != pressdata)
            {
                if (currentdata.isstartCo)
                {
                    currentdata.Stop_co();
                    currentdata = pressdata;
                }
                else
                    currentdata = pressdata;
            }
        }
        Debug.Log("Set_current_co 실행완료");
        yield return null;
    }
}