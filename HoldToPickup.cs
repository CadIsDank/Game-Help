using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoldToPickup : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Probably just the main camera.. but referenced here to avoid Camera.main calls")]
    private Camera camera;
    [SerializeField]
    [Tooltip("The layers the items to pickup will be on")]
    private LayerMask layerMask;
    [SerializeField]
    [Tooltip("How long it takes to pick up an item.")]
    private float pickupTime = 2f;
    [SerializeField]
    [Tooltip("The root of the images (progress image should be a child of this too)")]
    private RectTransform pickupImageRoot;
    [SerializeField]
    [Tooltip("The ring around the button that fills")]
    private Image pickupProgressImage;
    [SerializeField]
    private TextMeshProUGUI itemNameText;

    private Item itemBeingPickedUp;
    private float currentPickupTimerElapsed;

    GameObject windowCurtain;
    GameObject windowCurtain2;
    int canCount;
    int shirtCount;
    bool leftCurtain;
    bool rightCurtain;

    [SerializeField]
    private GameObject shadow;
    [SerializeField]
    private GameObject lampLight;
    [SerializeField]
    private GameObject screenOn;
    [SerializeField]
    private GameObject scare;
    [SerializeField]
    private GameObject streetLight;

    [SerializeField]
    private TextMeshProUGUI curtain;
    [SerializeField]
    private TextMeshProUGUI cans;
    [SerializeField]
    private TextMeshProUGUI shirts;
    [SerializeField]
    private TextMeshProUGUI dresser;
    [SerializeField]
    private TextMeshProUGUI curtain2;
    [SerializeField]
    private TextMeshProUGUI lamp;
    [SerializeField]
    private TextMeshProUGUI screen;
    [SerializeField]
    private TextMeshProUGUI fish;
    [SerializeField]
    private TextMeshProUGUI door;
    [SerializeField]
    private TextMeshProUGUI check;
    [SerializeField]
    private TextMeshProUGUI turnAround;

    private void Update()
    {
        SelectItemBeingPickedUpFromRay();

        if (HasItemTargetted())
        {
            pickupImageRoot.gameObject.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                IncrementPickupProgressAndTryComplete();
            }
            else
            {
                currentPickupTimerElapsed = 0f;
            }

            UpdatePickupProgressImage();
        }
        else
        {
            pickupImageRoot.gameObject.SetActive(false);
            currentPickupTimerElapsed = 0f;
        }
    }

    private bool HasItemTargetted()
    {
        return itemBeingPickedUp != null;
    }

    private void IncrementPickupProgressAndTryComplete()
    {
        currentPickupTimerElapsed += Time.deltaTime;
        if (currentPickupTimerElapsed >= pickupTime)
        {
            MoveItemToInventory();
        }
    }

    private void UpdatePickupProgressImage()
    {
        float pct = currentPickupTimerElapsed / pickupTime;
        pickupProgressImage.fillAmount = pct;
    }

    private void SelectItemBeingPickedUpFromRay()
    {
        Ray ray = camera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 3f, layerMask))
        {
            var hitItem = hitInfo.collider.GetComponent<Item>();

            if (hitItem == null)
            {
                itemBeingPickedUp = null;
            }
            else if (hitItem.CompareTag("End"))
            {
                SceneManager.LoadScene(0);
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
                //itemNameText.text = "Pickup " + itemBeingPickedUp.gameObject.name;
                itemNameText.text = itemBeingPickedUp.gameObject.name;
            }
        }
        else
        {
            itemBeingPickedUp = null;
        }
    }
    private void Check1()
    {
        if (curtain.enabled == false && cans.enabled == false && shirts.enabled == false && dresser.enabled == false)
        {
            curtain2.enabled = true;
            lamp.enabled = true;
            screen.enabled = true;
            fish.enabled = true;
            shadow.SetActive(true);
            screenOn.SetActive(false);
            lampLight.SetActive(false);
        }
    }
    private void Check2()
    {
        if (curtain2.enabled == false && lamp.enabled == false && screen.enabled == false && fish.enabled == false)
        {
            door.enabled = true;
            shadow.SetActive(false);
        }
    }
    private void Check3()
    {
        if (door.enabled == false)
        {
            check.enabled = true;
        }
    }
    private void Check4()
    {
        if (check.enabled == false)
        {
            turnAround.enabled = true;
            screenOn.SetActive(false);
            lampLight.SetActive(false);
            streetLight.SetActive(false);
            scare.SetActive(true);

        }
    }
    private void MoveItemToInventory()
    {
        if (itemBeingPickedUp.CompareTag("Lamp"))
        {

            /*if (lampLight == null)
            {
                lampLight = itemBeingPickedUp.transform.GetChild(0).gameObject;
            }*/
            if (lampLight.activeInHierarchy)
            {
                lampLight.SetActive(false);
            }
            else
            {
                lampLight.SetActive(true);
                if (lamp.enabled == true)
                {
                    lamp.enabled = false;
                    Check2();
                }
            }
        }
        else if (itemBeingPickedUp.CompareTag("Window"))
        {
            if (windowCurtain == null)
            {
                windowCurtain = itemBeingPickedUp.transform.GetChild(0).gameObject;
            }
            if (windowCurtain.activeInHierarchy)
            {
                windowCurtain.SetActive(false);
                leftCurtain = true;
                if (curtain.enabled == true)
                {
                    if (rightCurtain == true)
                    {
                        curtain.enabled = false;
                        Check1();
                    }
                }
                else if (check.enabled == true)
                {
                    if (rightCurtain == true)
                    {
                        check.enabled = false;
                        Check4();
                    }
                }
            }
            else
            {
                windowCurtain.SetActive(true);
                leftCurtain = false;
                if (curtain2.enabled == true)
                {
                    if (rightCurtain == false)
                    {
                        curtain2.enabled = false;
                        Check2();
                    }
                }
            }
        }

        else if (itemBeingPickedUp.CompareTag("Window2"))
        {
            if (windowCurtain2 == null)
            {
                windowCurtain2 = itemBeingPickedUp.transform.GetChild(0).gameObject;
            }
            if (windowCurtain2.activeInHierarchy)
            {
                windowCurtain2.SetActive(false);
                rightCurtain = true;
                if (curtain.enabled == true)
                {

                    if (leftCurtain == true)
                    {
                        curtain.enabled = false;
                        Check1();
                    }
                }
                if (check.enabled == true)
                {

                    if (leftCurtain == true)
                    {
                        check.enabled = false;
                        Check4();
                    }
                }
            }
            else
            {
                windowCurtain2.SetActive(true);
                rightCurtain = false;
                if (curtain2.enabled == true)
                {
                    if (leftCurtain == false)
                    {
                        curtain2.enabled = false;
                        Check2();
                    }
                }
            }
        }

        else if (itemBeingPickedUp.CompareTag("Screen"))
        {
            /*if (screenOn == null)
            {
                screenOn = itemBeingPickedUp.transform.GetChild(0).gameObject;
            }*/
            if (screenOn.activeInHierarchy)
            {
                screenOn.SetActive(false);
            }
            else
            {
                screenOn.SetActive(true);
                if (screen.enabled == true)
                {
                    screen.enabled = false;
                    Check2();
                }
            }
        }

        else if (itemBeingPickedUp.CompareTag("Can"))
        {
            canCount += 1;
            if (canCount > 2)
            {
                cans.enabled = false;
                Check1();
            }
            itemBeingPickedUp.gameObject.SetActive(false);
        }

        else if (itemBeingPickedUp.CompareTag("Shirt"))
        {
            shirtCount += 1;
            if (shirtCount > 3)
            {
                shirts.enabled = false;
                Check1();
            }
            itemBeingPickedUp.gameObject.SetActive(false);
        }

        else if (itemBeingPickedUp.CompareTag("Dresser"))
        {
            if (shirtCount > 3)
            {
                itemBeingPickedUp.gameObject.SetActive(false);
                dresser.enabled = false;
                Check1();
            }
            else
            {
                itemBeingPickedUp.gameObject.SetActive(true);
            }
        }

        else if (itemBeingPickedUp.CompareTag("Fish"))
        {
            if (fish.enabled == true)
            {
                fish.enabled = false;
                Check2();
            }
        }

        else if (itemBeingPickedUp.CompareTag("Door"))
        {
            if (door.enabled == true)
            {
                door.enabled = false;
                Check3();
            }
        }

        else
        {
            itemBeingPickedUp.gameObject.SetActive(false);
        }

        itemBeingPickedUp = null;
    }
}