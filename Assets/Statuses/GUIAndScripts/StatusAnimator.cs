using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
public class StatusAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<StatusVisual> statusImages=new List<StatusVisual>();
    public GameObject StatusObjectPrefab;
    public Transform enemyStatusPanel;
     int Rowheight=3;
     void Start()
     {
         StartCoroutine(LateStart(0.1f));
     }
 
     IEnumerator LateStart(float waitTime)
     {
        //Subscribe to event when created
         yield return new WaitForSeconds(waitTime);
         init();
     }
     public void init(){
        Deck.Instance.statuses.CollectionChanged+=new NotifyCollectionChangedEventHandler(RecalculateStatusList);

     }
    public void RecalculateStatusList(object sender, NotifyCollectionChangedEventArgs e){
       // while(statusImages.Count!=0){
       //     var temp=statusImages[0];
       //     statusImages.RemoveAt(0);
       //     Destroy(temp);
       // }
        //statusImages=new List<GameObject>();
        Debug.Log("Status Registered");
        if(e.NewItems!=null){
        foreach(StatusEffectInstance i in e.NewItems){
            var stat=Instantiate(StatusObjectPrefab);
            var s = stat.GetComponent<StatusVisual>();
            s.init(i,this);
            stat.transform.position=this.transform.position+new Vector3(statusImages.Count*4,0,0);
            statusImages.Add(s);
            Debug.Log("Status added");
        }
        }
    }
    void OnDestroy() {
        //Unsubscribe from event when destroyd
        Deck.Instance.statuses.CollectionChanged-=RecalculateStatusList;
    }
    // Update is called once per frame
    float distance=4f;
    bool dir=true;
    float t1;
    float tmax=1.5f;
    float Bounce(float t){
        if(dir){

        if(t1<tmax){
            return (NonLinInterpolationUtil.Interpolate((t1/tmax),NonLinInterpolationUtil.EaseType.Quadratic,NonLinInterpolationUtil.EaseType.Quadratic));            
        }else{
            t1=0.0001f;
            dir=false;
        return 1-(NonLinInterpolationUtil.Interpolate((t1/tmax),NonLinInterpolationUtil.EaseType.Quadratic,NonLinInterpolationUtil.EaseType.Quadratic));

        }

        }else{
            if(t1<tmax){
        return 1-(NonLinInterpolationUtil.Interpolate((t1/tmax),NonLinInterpolationUtil.EaseType.Quadratic,NonLinInterpolationUtil.EaseType.Quadratic));
             }else{
                t1=0.0001f;
                dir=true;
return (NonLinInterpolationUtil.Interpolate((t1/tmax),NonLinInterpolationUtil.EaseType.Quadratic,NonLinInterpolationUtil.EaseType.Quadratic));            

            }
            
        
        }
    }
    void Update()
    {
        t1+=Time.deltaTime;
        int index=0;
        int enemyIndex=0;
        int enemyimages = 0;
        int images = 0;
        foreach (var i in statusImages)
        {
            if (i.stat.targetsEnemy)
            {
                enemyimages++;
            }
            else
            {
                images++;
            }
        }
            foreach (var i in statusImages){
            var y=(i.transform.position.y-transform.position.y);
            y=Bounce(y);
            //Debug.Log(y);
            if (!i.stat.targetsEnemy)
            {
                i.transform.position = new Vector3((transform.position.x - ((Mathf.Min(3, images) / 2.0f) * distance)) + ((index % 3) * distance), (index / 3) * Rowheight + transform.position.y + y, transform.position.z);
                //i.transform.position = new Vector3(transform.position.x - (((images%3 )/ 2.0f) * distance) + (index * distance), (index/3)*distance+transform.position.y + y, transform.position.z);
                index++;
            }
            else {
                i.transform.position = new Vector3((enemyStatusPanel.position.x - ((Mathf.Min(3,enemyimages) / 2.0f) * distance)) + ((enemyIndex % 3) * distance), (enemyIndex / 3) * Rowheight + enemyStatusPanel.position.y+y , enemyStatusPanel.position.z);
                enemyIndex++;
            }
        }
        
    }
}
