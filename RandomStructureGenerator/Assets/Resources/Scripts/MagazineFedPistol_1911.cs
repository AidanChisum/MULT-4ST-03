using UnityEngine;
using VRTK;

public class MagazineFedPistol_1911 : VRTK_InteractableObject
{
    public float bulletSpeed = 200f;
    public float bulletLife = 5f;

    private GameObject bullet;
    private GameObject trigger;
    private MagazineFedPistol_Slide slide;

    private Rigidbody slideRigidbody;
    private Collider slideCollider;

    private VRTK_ControllerEvents controllerEvents;

    private float minTriggerRotation = -10f;
    private float maxTriggerRotation = 45f;

    private void ToggleCollision(Rigidbody objRB, Collider objCol, bool state)
    {
        objRB.isKinematic = state;
        objCol.isTrigger = state;
    }

    private void ToggleSlide(bool state)
    {
        if (!state)
        {
            slide.ForceStopInteracting();
        }
        slide.enabled = state;
        slide.isGrabbable = state;
        ToggleCollision(slideRigidbody, slideCollider, state);
    }

    public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
    {
        base.Grabbed(currentGrabbingObject);

        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();

        ToggleSlide(true);

        //Limit hands grabbing when picked up
        if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject.controllerEvents.gameObject) == SDK_BaseController.ControllerHand.Left)
        {
            allowedTouchControllers = AllowedController.LeftOnly;
            allowedUseControllers = AllowedController.LeftOnly;
            slide.allowedGrabControllers = AllowedController.RightOnly;
        }
        else if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject.controllerEvents.gameObject) == SDK_BaseController.ControllerHand.Right)
        {
            allowedTouchControllers = AllowedController.RightOnly;
            allowedUseControllers = AllowedController.RightOnly;
            slide.allowedGrabControllers = AllowedController.LeftOnly;
        }
    }

    public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);

        ToggleSlide(false);

        //Unlimit hands
        allowedTouchControllers = AllowedController.Both;
        allowedUseControllers = AllowedController.Both;
        slide.allowedGrabControllers = AllowedController.Both;

        controllerEvents = null;
    }

    public override void StartUsing(VRTK_InteractUse currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        
        slide.Fire();
        FireBullet();
        VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 0.63f, 0.2f, 0.01f);
    }

    protected override void Awake()
    {
        base.Awake();
        bullet = transform.Find("Cartridge").gameObject;
        bullet.SetActive(false);

        trigger = transform.Find("Trigger").gameObject;

        slide = transform.Find("Slide").GetComponent<MagazineFedPistol_Slide>();
        slideRigidbody = slide.GetComponent<Rigidbody>();
        slideCollider = slide.GetComponent<Collider>();

    }

    protected override void Update()
    {
        base.Update();
        if (controllerEvents)
        {
            var pressure = (maxTriggerRotation * controllerEvents.GetTriggerAxis()) - minTriggerRotation;
            trigger.transform.localEulerAngles = new Vector3(0f, pressure, 0f);
        }
        else
        {
            trigger.transform.localEulerAngles = new Vector3(0f, minTriggerRotation, 0f);
        }
    }

    private void FireBullet()
    {
        GameObject bulletClone = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation) as GameObject;
        bulletClone.SetActive(true);
        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        rb.AddForce(bullet.transform.forward * bulletSpeed);
        Destroy(bulletClone, bulletLife);
    }
}