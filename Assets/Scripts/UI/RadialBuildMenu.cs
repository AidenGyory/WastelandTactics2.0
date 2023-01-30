using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialBuildMenu : MonoBehaviour
{
    

    public GameObject SelectedWorkerUnit;
    // Rference to Radial Menu
    [SerializeField] GameObject RadialMenu;

    //reference to Move input
    Vector2 moveInput;

    // References to external options
    [SerializeField] GameObject[] options;
    [SerializeField] GameObject exitButton;
    [SerializeField] int optionSelected;
    // 360 / number of options 
    [SerializeField] float optionsThreshold;

    //colours for text
    [SerializeField] Color standard, highlight;
    [Header("Center Text")]
    [SerializeField] TMP_Text structureSelectedText;
    [SerializeField] Image selectedIcon;

    

    public void OpenRadialMenu()
    {
        RadialMenu.SetActive(true);
    }

    public void CloseRadialMenu()
    {
        SelectObjectScript.Instance.SetModeToSelect();
        RadialMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RadialMenu.activeInHierarchy)
        {
            moveInput.x = Input.mousePosition.x - (Screen.width / 2f);
            moveInput.y = Input.mousePosition.y - (Screen.height / 2f);

            float _distance = Vector2.Distance(Vector2.zero, moveInput);
            //Debug.Log(_distance);

            moveInput.Normalize();



            if (moveInput != Vector2.zero)
            {


                float _angle = Mathf.Atan2(moveInput.y, -moveInput.x) / Mathf.PI;
                _angle *= 180f;
                _angle -= 90f;
                if (_angle < 0)
                {
                    _angle += 360;
                }

                if (_distance < 90 || _distance > 220)
                {

                    optionSelected = 0;
                    selectedIcon.color = Color.clear;
                }


                //Debug.Log(_angle);

                for (int i = 0; i < options.Length; i++)
                {
                    //segments are 360 / number of options 
                    if (_angle > i * optionsThreshold && _angle < (i + 1) * optionsThreshold && _distance > 90 && _distance < 220)
                    {
                        //select option
                        optionSelected = i;
                        options[i].GetComponent<RadialOptionScript>().HighlightSegment(highlight);
                        structureSelectedText.text = "" + options[i].GetComponent<RadialOptionScript>().unitName;

                        selectedIcon.sprite = options[i].GetComponent<RadialOptionScript>().icon.sprite;
                        selectedIcon.color = new Color(1, 1, 1, 0.7f);


                    }
                    else
                    {
                        options[i].GetComponent<RadialOptionScript>().UnHighlightSegment(standard);
                    }
                }


            }
            if (Input.GetMouseButtonDown(0))
            {
                if (_distance < 90 || _distance > 220)
                {

                    SelectObjectScript.Instance.canSelect = true;
                    CloseRadialMenu();
                }
                else
                {
                    switch (optionSelected)
                    {
                        case 0: // Outpost 
                            PlaceStructure(WorkerUnit.StructureType.Outpost); 
                            break;
                        case 1: // Factory
                            PlaceStructure(WorkerUnit.StructureType.Factory); 
                            break;
                        case 2: // Cancel 
                            CloseRadialMenu();
                            break;
                        case 3: // PowerCell
                            PlaceStructure(WorkerUnit.StructureType.PowerCell); 
                            break;
                        case 4: // Research
                            PlaceStructure(WorkerUnit.StructureType.Research); 
                            break;
                    }
                }
            }
        }
    }

    public void PlaceStructure(WorkerUnit.StructureType _structureIndex)
    {
        if (SelectedWorkerUnit.GetComponent<WorkerUnit>().CheckIfSuitablePlaceForStructure(_structureIndex))
        {
            GameObject _placeable = Instantiate(SelectedWorkerUnit.GetComponent<WorkerUnit>().PlaceableStructures[(int)_structureIndex]);
            _placeable.GetComponent<StructureInfo>().owner = GameManager.Instance.playerInfo[(int)GameManager.Instance.currentPlayerTurn];
            _placeable.transform.position = SelectedWorkerUnit.transform.position;
            _placeable.transform.parent = null;
            _placeable.GetComponent<StructureInfo>().UpdatePlayerDetails();

            CloseRadialMenu();
            Destroy(SelectedWorkerUnit);
           
        }
        else
        {
            TileAudioManager.instance.PlayTileAudio(tileAudioType.negflip);
            Debug.Log("Can't Build that here!");
            
        }

        
    }
}
