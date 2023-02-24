using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
public class StatusAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> statusImages=new List<GameObject>();
    public GameObject StatusObjectPrefab;

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
            stat.GetComponent<StatusVisual>().init(i,this);
            stat.transform.position=this.transform.position+new Vector3(statusImages.Count*4,0,0);
            statusImages.Add(stat);
            Debug.Log("Status added");
        }
        }
    }
    void OnDestroy() {
        //Unsubscribe from event when destroyd
        Deck.Instance.statuses.CollectionChanged-=RecalculateStatusList;
    }
    // Update is called once per frame
    float distance=2f;
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
        foreach(var i in statusImages){
            var y=(i.transform.position.y-transform.position.y);
            y=Bounce(y);
            //Debug.Log(y);
            i.transform.position=new Vector3(transform.position.x-((statusImages.Count/2.0f)*distance)+(index*distance),transform.position.y+y,transform.position.z);
            index++;
        }
        
    }
}
