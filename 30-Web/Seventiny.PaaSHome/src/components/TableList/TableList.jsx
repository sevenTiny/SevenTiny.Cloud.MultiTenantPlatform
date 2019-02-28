import React, { Component } from 'react';
import { Table, Pagination } from '@alifd/next';
import IceContainer from '@icedesign/container';
import dataApiHost from '../../commonConfig';
import axios from 'axios';

//请求列表数据的地址
const tableDataUri = dataApiHost + '/api/UI/TableList?viewName=WangDongApp.DataTest.IndexView.TestIndexView';

const random = (min, max) => {
  return Math.floor(Math.random() * (max - min + 1) + min);
};

const state = [
  {
    color: '#999',
    text: '已批准',
  },
  {
    color: '#ee706d',
    text: '流程中',
  },
  {
    color: '#5e83fb',
    text: '已撤回',
  },
];

export default class TableList extends Component {

  state = {
    current: 1,
    isLoading: false,
    data: [],
    totalCount: 0,
    columnData: [],
    widths: {},
    //默认页大小
    pageSize: 15
  };

  //属性被更改时触发
  shouldComponentUpdate(newProps) {
    //如果属性被更新
    if (newProps !== this.props) {
      this.fetchData()
    }
    return true;
  }

  componentDidMount() {
    this.fetchData();
  }

  // MOCK 数据，实际业务按需进行替换
  getData = () => {
    var th = this;
    var bizDatas = [];

    //请求数据
    axios({
      method: 'post',
      url: tableDataUri,
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

  // mockApi = (len) => {
  //   return new Promise((resolve) => {
  //     resolve(this.getData(len));
  //   });
  // };

  fetchData = () => {
    this.setState(
      {
        isLoading: true,
      },
      () => {
        this.getData();
        //由于异步返回结果，所以下面同步的方法是执行效果有误的
        // .then((data) => {
        //   this.setState({
        //     data,
        //     isLoading: false,
        //   });
        // });
      }
    );
  };

  handlePaginationChange = (current) => {
    this.setState(
      {
        current,
      },
      () => {
        this.fetchData();
      }
    );
  };

  //页大小切换效果
  onPageSizeChange = (num) => {
    this.setState({ pageSize: num })
    this.fetchData()
  }

  //表格列拖拽增加列宽度效果
  onResizeChange = (dataIndex, value) => {
    const { widths } = this.state;
    widths[dataIndex] = widths[dataIndex] + value;
    this.setState({
      widths
    });
  }

  handleFilterChange = () => {
    this.fetchData();
  };

  renderState = (value) => {
    return (
      <span
        style={{
          background: value.color,
          color: '#fff',
          padding: '4px 8px',
          borderRadius: '4px',
          fontSize: '12px',
        }}
      >
        {value.text}
      </span>
    );
  };

  render() {
    const { isLoading, data, current } = this.state;

    return (
      <IceContainer style={styles.container}>
        <Table
          loading={isLoading}
          dataSource={data}
          hasBorder={false}
          style={{ padding: '0 20px 20px', whiteSpace: 'nowrap' }}
          useVirtual
          fixedHeader={true}
          maxBodyHeight={window.innerHeight * 0.6}
          onResizeChange={this.onResizeChange}
        >
          {
            this.state.columnData.map((item, index) => {
              return <Table.Column title={item.cmp_data.title} key={''} dataIndex={item.cmp_data.name} width={this.state.widths[item.cmp_data.name]} resizable />
            })
          }
          {/* <Table.Column title="状态" dataIndex="state" cell={this.renderState} /> */}
        </Table>
        <Pagination
          style={styles.pagination}
          current={current}
          onChange={this.handlePaginationChange}
          pageSize={this.state.pageSize}
          pageSizeSelector="dropdown"
          pageSizePosition="end"
          pageSizeList={[15, 30, 60, 100]}
          total={this.state.totalCount}
          totalRender={total => `共${total}条`}
          onPageSizeChange={this.onPageSizeChange}
        />
      </IceContainer>
    );
  }
}

const styles = {
  container: {
    padding: '0 0 20px',
  },
  pagination: {
    textAlign: 'right',
  },
};
