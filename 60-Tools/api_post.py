import requests
import datetime

url = 'http://localhost:57004/api/CloudData?metaObjectCode=WangDongApp.DataTest'

i = 123123

data = {
    'positiveNumber': i,
    'Text': 'Text测试'+str(i),
    'DateTime': str(datetime.datetime.now()),
    'TrueOrFalse': True,
    'Integer': i,
    'Long': i,
    'Double': i
}
response = requests.post(url, json=data)
print(response.text)
