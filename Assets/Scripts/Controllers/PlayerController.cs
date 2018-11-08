using UnityEngine;
using System.Collections;
using UnityEngine.UI;



namespace UnitySampleAssets.CrossPlatformInput.PlatformSpecific
{
public class PlayerController : MonoBehaviour {

	public float maxSpeed = 10.0f;


	//VARIABLES FOR CONTROLLING THE RELOADING BEHABIOUR.
	/*private float reloadElapsedTime = 0.0f;
	private bool isReloading = false;
	public float timeNeededForReloading = 1.0f;
	public AudioClip reloadSFX;
	public float reloadSFXElaplsedTime;


	Animator anim;

	public AudioClip footStep;
	private float footStepElapsedTime;
	public float footStepAllowTime = 0.43f;


	public int maxBulletCarryingCapacity = 10;
	public int currentNumberOfBullet;

	*/

	float horizontal;
	float vertical;

	float shootingHorizontal;
	float shootingVertical;

	public int playerHealth = 100;
	
	//UI
	private Image playerHeathUI;
	private Text playerHealthTextUI;


	//Player Dead
	public bool isPlayerDead = false;
	private float gameOverScreenLoadDelayTime = 2.0f;
	private float gameOverScreenLoadElapsedTime = 0.0f;

	public bool isPowerUpActive = false;
	public GameObject shieldPowerUp;
	private float powerUpElapsedTime = 0;

	void Start () {
			/*anim = GetComponent<Animator> ();
			currentNumberOfBullet = maxBulletCarryingCapacity;

			playerHeathUI = GameObject.Find ("HealthUI").GetComponent<Image> ();
			playerHealthTextUI = GameObject.Find ("scoreTextUI").GetComponent<Text> ();

		*/

	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "bullet") {

				Destroy(other.gameObject);
				playerHealth -= 40;
				if(playerHealth <0)playerHealth = 0;
				updateHealthBarAndText();


		}
	}

	private void updateHealthBarAndText(){
			playerHealthTextUI.text = playerHealth.ToString ();
			playerHeathUI.fillAmount = (float)playerHealth/100.0f;
	}
	
	private void loadGameOverScene(){
		Application.LoadLevel("GameOver");
	}

	
	void Update(){
			rigidbody2D.AddForce (new Vector2 (4000.0f * horizontal, 4000.0f * vertical) * Time.deltaTime);
			if (isPowerUpActive == true) {
				powerUpElapsedTime += Time.deltaTime;
				if(powerUpElapsedTime >= 4.0f){
					isPowerUpActive = false;
					shieldPowerUp.SetActive (false);
					powerUpElapsedTime = 0;
				}
			}
	}


	public void onPowerUpCollection(){
			shieldPowerUp.SetActive (true);
			isPowerUpActive = true;
			powerUpElapsedTime = 0;
			//Debug.Log ("From here...");
	}


	void FixedUpdate () {

		//X AXIS MOVEMENT AND ANIMATION
//		float move = Input.GetAxis ("Horizontal");
//		anim.SetFloat ("Speed",Mathf.Abs(move));
//		rigidbody2D.velocity = new Vector2(move*maxSpeed,rigidbody2D.velocity.y);

			horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			vertical = CrossPlatformInputManager.GetAxis("Vertical");

			/*shootingHorizontal = CrossPlatformInputManager.GetAxis("Shooting_Horizontal");
			shootingVertical = CrossPlatformInputManager.GetAxis("Shooting_Vertical");

			if (shootingHorizontal != 0) {
				anim.SetBool ("Eating", true);
			} else {
				anim.SetBool("Eating", false);
			}*/
			
			if(horizontal!=0 || vertical!=0){
				//if(shootingHorizontal ==0 && shootingVertical ==0){
					var objectPos = transform.position;
					var dir = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"),0.0f) - new Vector3(0,0,0); 
					transform.rotation = Quaternion.Euler (new Vector3(0,0,Mathf.Atan2 (dir.y,dir.x) * Mathf.Rad2Deg));

				//}
//				Debug.Log("H: " + horizontal + " V: " +vertical);
				
				//rigidbody2D.AddForce (transform.right* maxSpeed*Time.deltaTime);
				
				
			}

	


		




		//**********TEMPORARY CONTROLS FOR PC **************
		/*var mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Quaternion rot = Quaternion.LookRotation (transform.position - mousePosition, Vector3.forward);

		transform.rotation = rot;
		transform.eulerAngles = new Vector3(0,0,transform.eulerAngles.z+90);
		rigidbody2D.angularVelocity = 0;

		float moveInput = Input.GetAxis ("Vertical");
		anim.SetFloat ("Speed",Mathf.Abs(moveInput));
		if (Mathf.Abs (moveInput) > 0) {
			if(footStepElapsedTime > footStepAllowTime) {audio.PlayOneShot(footStep);footStepElapsedTime=0;}
			else{
				footStepElapsedTime += Time.deltaTime;
			}
		}
		rigidbody2D.AddForce (gameObject.transform.right*moveInput * maxSpeed);*/


	}
}

}
