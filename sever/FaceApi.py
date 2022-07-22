import cognitive_face as CF
import math
def FaceCheck(_pass):
    result_text=""
    KEY = '28537106a1f9455bad12af8c063cc2d0'
    BASE_URL = 'https://japaneast.api.cognitive.microsoft.com/face/v1.0'
    CF.Key.set(KEY)
    CF.BaseUrl.set(BASE_URL)
    faces = CF.face.detect(_pass, attributes='emotion')
    faces_dict=faces[0]['faceAttributes']['emotion']
    sort_dict=sorted(faces_dict.items(),key=lambda x:x[1],reverse=True)
    for _list in sort_dict:
        #_listがタプルになってしまいSplitができないため文字列化
        sort_list = list(_list)
        if sort_list[1]!=0.0:
            result_text += ChangeText(sort_list[0])+","+ResultText(sort_list[1])+","
    return result_text

def ChangeText(_text):
    r_text=""
    chexk_list=["anger,怒り","contempt,軽蔑","disgust,嫌悪","fear,恐れ","happiness,幸福","neutral,平常","sadness,悲しみ","surprise,驚き"]
    for item in chexk_list:
        sprit_text=item.split(',')
        if _text==sprit_text[0]:
            r_text=sprit_text[1] 
    return r_text

def ResultText(_num):
    re_text=""
    res_text=str(math.floor(_num*10**3)/(10**3))
    if _num > 0.5:
        re_text="高(スコア："+res_text+")"
    elif _num > 0.2:
        re_text="中(スコア："+res_text+")"
    else:
        re_text="低(スコア："+res_text+")"
    return re_text
if __name__ == '__main__':
    FaceCheck()
