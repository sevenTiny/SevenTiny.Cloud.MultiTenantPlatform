import requests
import datetime

url = 'http://localhost:57004/api/BatchCloudData?metaObjectCode=WangDongApp.DataTest'

datas = []
for i in range(11000,20000):
    data = {
        'positiveNumber': i,
        'Text': 'Text测试'+str(i),
        'DateTime': str(datetime.datetime.now()),
        'TrueOrFalse': True,
        'Integer': i,
        'Long': i,
        'Double': i
    }
    datas.append(data)
    print(i)

response = requests.post(url, json=datas)
print(response.text)
