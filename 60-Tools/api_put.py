import requests
import datetime

url = 'http://localhost:57004/api/CloudData?conditionCode=WangDongApp.DataTest.SearchCondition.GetByInteger&Integer=90000'

i = 90000

data = {
    # 'positiveNumber': i,
    # 'Text': 'Text测试'+str(i),
    # 'DateTime': str(datetime.datetime.now()),
    # 'TrueOrFalse': True,
    # 'Integer': i,
    'Long': i,
    'Double': 90088
}
response = requests.put(url, json=data)
print(response.text)
