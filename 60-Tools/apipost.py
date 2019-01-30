import requests
import datetime

url = 'http://localhost:57004/api/CloudData?metaObjectCode=WangDongApp.DataTest'

for i in range(0,1000):
    data = {
        'positiveNumber': i,
        'Text': 'Text测试',
        'DateTime': str(datetime.datetime.now()),
        'TrueOrFalse': True,
        'Integer': i,
        'Long': i,
        'Double': i
    }
    response = requests.post(url, json=data)
    print(i)
    print(response.text)
