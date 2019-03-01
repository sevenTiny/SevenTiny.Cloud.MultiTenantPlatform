import React, { Component } from 'react';
import IceContainer from '@icedesign/container';
import './IndexView.scss';
import TableList from '../TableList';
import SearchForm from '../SearchForm';
import dataApiHost from '../../commonConfig';

//请求视图页组件的接口
const indexComponentApi = dataApiHost + '/api/UI/IndexPage?viewName=WangDongApp.DataTest.IndexView.TestIndexView';

const styles = {
  container: {
    padding: '0 0 20px',
  },
  title: {
    margin: '0',
    padding: '15px 20px',
    fonSize: '16px',
    textOverflow: 'ellipsis',
    overflow: 'hidden',
    whiteSpace: 'nowrap',
    color: 'rgba(0,0,0,.85)',
    fontWeight: '500',
    borderBottom: '1px solid #eee',
  },
  pagination: {
    textAlign: 'right',
  },
};

export default class IndexView extends Component {
  constructor(props) {
    super(props);
    this.state = {
      searchDatas: null
    }
  }

  // MOCK 数据，实际业务按需进行替换
  getData = () => {
    var th = this;
    var bizDatas = [];

    //请求数据
    axios({
      method: 'get',
      url: indexComponentApi,
      data: {
        ViewName: this.props.ViewName,
        MetaObject: this.props.MetaObject,
        Application: null,
        SearchData: {
          SearchView: null,
          Items: this.props.searchDatas
        },
        SortFields: null,
        PageIndex: this.state.current,
        PageSize: this.state.pageSize
      }
    }).then(function (response) {
      if (response.status == 200) {
        if (response.data.success) {
          //给请求到的数据属性赋值给table组件
          var result = response.data.data
          var widths = {}
          bizDatas = result.biz_data.map((item, index) => {
            var obj = {}
            for (var columnItem in result.sub_cmps) {
              //列编码
              var columnName = result.sub_cmps[columnItem].cmp_data.name;
              //添加该列的字段
              obj[columnName] = item[columnName].text;
              widths[columnName] = item[columnName].width == null ? 100 : item[columnName].width;
            }
            //宽度赋值
            return obj;
          });
          th.setState({
            data: bizDatas,
            isLoading: false,
            totalCount: result.biz_data_total_count,
            columnData: result.sub_cmps,
            widths: widths
          });
        }
      } else {
        th.setState({
          isLoading: true,
        });
        console.error(response)
      }
    });
    return bizDatas;
  };

  //条件改变重新赋值
  conditionChange = (value) => {
    //创建搜索条件数组
    var searchDatas = [];
    for (var item in value) {
      if (value[item] != null && value[item] != '') {
        searchDatas.push({
          Name: item,
          Text: null,
          Value: value[item]
        });
      }
    }
    this.setState({
      searchDatas: searchDatas
    })
  };

  render() {
    return (
      <IceContainer style={styles.container}>
        <h4 style={styles.title}>请假记录</h4>
        <SearchForm onChange={this.conditionChange} />
        <TableList searchDatas={this.state.searchDatas} ViewName={this.props.ViewName} MetaObject={this.props.MetaObject} />
      </IceContainer>
    );
  }
}

