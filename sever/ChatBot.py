import requests
from IPython.display import Image

apikey = "DZZe9MnfHOfdU1FuJ10YNqWpqQWjuFhM"
talk_url = "https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk"

def talk_api(message):

    payload = {"apikey": apikey, "query": message}
    response = requests.post(talk_url, data=payload)
    try:
        return response.json()["results"][0]["reply"]
    except:
        print(response.json())
        return "ごめんなさい。もう一度教えて下さい。"