using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    private SpriteRenderer spriteRenderer;

    List<GameObject> ammoDisplayed = new List<GameObject>();
    List<GameObject> ammoEmptyDisplayed = new List<GameObject>();
    public List<GameObject> rechargesDisplayed = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        for(int i = 0; i < currentWeapon.maxAmmo; i ++){
            GameObject go = Instantiate(currentWeapon.ammoSpriteForUI, transform.parent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(15 + i*(go.GetComponent<RectTransform>().sizeDelta.x + 5), -go.GetComponent<RectTransform>().sizeDelta.y/2 -50);
            ammoDisplayed.Add(go);
        }
        for(int i = 0; i < currentWeapon.maxAmmo; i ++){
            GameObject go = Instantiate(currentWeapon.ammoEmptySpriteForUI, transform.parent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(15 + i*(go.GetComponent<RectTransform>().sizeDelta.x + 5), -go.GetComponent<RectTransform>().sizeDelta.y/2 -50);
            ammoEmptyDisplayed.Add(go);
            go.SetActive(false);
        }
        float currentDistance = 15 + (currentWeapon.maxAmmo+1)*(currentWeapon.ammoSpriteForUI.GetComponent<RectTransform>().sizeDelta.x + 5);
        for(int i = 0; i < currentWeapon.maxRecharges; i ++){
            GameObject go = Instantiate(currentWeapon.rechargeSpriteForUI, transform.parent);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentDistance + 15 + i*(go.GetComponent<RectTransform>().sizeDelta.x + 5), -go.GetComponent<RectTransform>().sizeDelta.y/2 -50);
            rechargesDisplayed.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < currentWeapon.maxAmmo; i ++){
            if(i < currentWeapon.currentAmmo){
                ammoDisplayed[i].SetActive(true);
                ammoEmptyDisplayed[i].SetActive(false);
            }
            else{
                ammoDisplayed[i].SetActive(false);
                ammoEmptyDisplayed[i].SetActive(true);
            }
        }
        for(int i = 0; i < currentWeapon.maxRecharges; i ++){
            if(i < currentWeapon.currentRecharges){
                rechargesDisplayed[i].SetActive(true);
            }
            else{
                rechargesDisplayed[i].SetActive(false);
            }
        }
    }
}
