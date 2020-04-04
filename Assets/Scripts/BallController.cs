using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody rb;  // component rigidbody
    [SerializeField] float speed; // tốc độ
    bool start; // check game start
    bool gameOver; // check gameOver
    public GameObject Partical; // particel system
    [SerializeField] AudioSource audio;
    // Start is called before the first frame update
    private void Awake()
    {
        start = false;
        gameOver = false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>(); //chiếu tới component(tham trị hay tìm địa chỉ) rigidbody trong đối tượng Ball
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down,Color.red);
        if(!Physics.Raycast(transform.position,Vector3.down,1f))  // nếu raycast ko va chạm cái gì thì coi như là rơi -> giả lập trong lực 
        {
            gameOver = true;
            rb.velocity = new Vector3(0, -25f, 0);   // rơi với tốc độ 25f
            Camera.main.GetComponent<CameraFollow>().gameOver = true; // camera không theo nữa
            GameManager.instance.GameOver();        // gọi đối tượng game để kết thúc
        }
        if(!start)   //nếu chưa bắt đầu
        {
            if(Input.GetMouseButtonDown(0))  //ấn phím chuột bất kỳ  -> trên android là màn hình
            {
                rb.velocity = new Vector3(speed, 0, 0)*Time.deltaTime; // gán tốc độ cho rigid body  Time.deltaTime cân bằng độ delay giữ các frame 
                start = !start;     // bắt đàu game
                GameManager.instance.GameStart(); // gọi đối tượng game để bắt đầu
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0)&& !gameOver) // ấn lên màn hinh và game đang diễn ra
            { 
                SwitchDirection();  // đổi hướng 
            }
        }
        Debug.Log("Runnnnnnnnnn");
    }

    private void SwitchDirection()  // hàm đổi hướng
    {
        if(rb.velocity.z > 0) //nếu vật di chuyển theo trục z
        {
            rb.velocity = new Vector3(speed, 0, 0)*Time.deltaTime;  // vật di chuyển theo hướng z  
        }else if(rb.velocity.x > 0) //nếu vật di chuyển theo trục x
        {
            rb.velocity = new Vector3(0, 0, speed)*Time.deltaTime; // vật di chuyển theo trục z
        }
    }
    private void OnTriggerEnter(Collider other) // xét trigger  (là ý nói va chạm nhưng hai vật ko thực hiện quy luật tương tác với nhau, chỉ xét hai vậy có đi qua nhau không)
    {
        if(other.gameObject.tag == "diamond") // nếu object va chạm có tag là diamond
        {
            ScoreManager.instance.addBonusScore();
            GameObject part = Instantiate(Partical, other.gameObject.transform.position, Quaternion.identity) as GameObject; // khởi tạo đối tượng ở đây là particle system
            Destroy(other.gameObject); // phá hủy đối tượng va chạm
            Destroy(part, 1f); // phá hủy đối tượng particle system
            audio.clip = GameManager.instance.list[1];
            audio.PlayOneShot(audio.clip);
        }
    }
}

//  50 f trong 1 s time.delta laf delay giua 2 frame
// 50  time.deltea giua la 5
// 300 frame time.deasdf giua 2 frame la 1.5