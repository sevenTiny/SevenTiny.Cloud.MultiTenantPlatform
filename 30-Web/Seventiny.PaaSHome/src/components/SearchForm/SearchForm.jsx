/* eslint react/no-string-refs:0 */
import React, { Component } from 'react';
import { Button, Grid, DatePicker, Select, Input } from '@alifd/next';
import {
  FormBinderWrapper as IceFormBinderWrapper,
  FormBinder as IceFormBinder,
  FormError as IceFormError,
} from '@icedesign/form-binder';

const { Row, Col } = Grid;

export default class SearchForm extends Component {
  constructor(props) {
    super(props)
    this.state = {
      searchItems: this.props.searchItems
    }
  }

  state = {
    value: {},
    searchItems: [],
  };

  search = () => {
    this.props.onChange(this.state.value)
  }

  formChange = (value) => {
    this.setState({ value: value })
  }

  render() {
    return (
      <IceFormBinderWrapper
        value={this.state.value}
        onChange={this.formChange}
        ref="form"
      >
        <Row wrap gutter="20" style={styles.formRow}>
          {this.state.searchItems.map((item, index) => {

            let formItem = null
            if (true) {
              formItem =
                <Input
                  hasClear
                  defaultValue='123123'
                  placeholder="请输入" />
            }

            return (
              <Col l="8" key={index} hidden={!item.visible}>
                <div style={styles.formItem} >
                  <span style={styles.formLabel}>{item.text}：</span>
                  <IceFormBinder triggerType="onBlur" name={item.name} value="333" >
                    {formItem}
                  </IceFormBinder>
                  <div style={styles.formError}>
                    <IceFormError name={item.name} />
                  </div>
                </div>
              </Col>)
          })
          }
          <Col l="8">
            <div style={styles.formItem}>
              <span style={styles.formLabel}>开始日期：</span>
              <Input
                // hasClear
                defaultValue='123123'
                value='123123'
                placeholder="请输入11233" />

            </div>
          </Col>
          <Col l="8">
            <div style={styles.formItem}>
              <Button type="secondary" onClick={this.search}>搜索</Button>
            </div>
          </Col>
          {/* <Col l="8">
            <div style={styles.formItem}>
              <span style={styles.formLabel}>开始日期：</span>
              <IceFormBinder triggerType="onBlur" name="startTime">
                <DatePicker placeholder="请输入" />
              </IceFormBinder>
              <div style={styles.formError}>
                <IceFormError name="startTime" />
              </div>
            </div>
          </Col>
          <Col l="8">
            <div style={styles.formItem}>
              <span style={styles.formLabel}>结束日期：</span>
              <IceFormBinder triggerType="onBlur" name="endTime">
                <DatePicker placeholder="请输入" />
              </IceFormBinder>
              <div style={styles.formError}>
                <IceFormError name="endTime" />
              </div>
            </div>
          </Col>
          <Col l="8">
            <div style={styles.formItem}>
              <span style={styles.formLabel}>假期类型：</span>
              <IceFormBinder triggerType="onBlur" name="type">
                <Select name="type" style={{ width: '200px' }}>
                  <Select.Option value="1">休年假</Select.Option>
                  <Select.Option value="2">事假</Select.Option>
                  <Select.Option value="3">调休</Select.Option>
                </Select>
              </IceFormBinder>
              <div style={styles.formError}>
                <IceFormError name="type" />
              </div>
            </div>
          </Col> */}
        </Row>
      </IceFormBinderWrapper>
    );
  }
}

const styles = {
  title: {
    margin: '0',
    padding: '20px',
    fonSize: '16px',
    textOverflow: 'ellipsis',
    overflow: 'hidden',
    whiteSpace: 'nowrap',
    color: 'rgba(0,0,0,.85)',
    fontWeight: '500',
    borderBottom: '1px solid #eee',
  },
  formRow: {
    padding: '10px 20px',
  },
  formItem: {
    display: 'flex',
    alignItems: 'center',
    margin: '10px 0',
  },
  formLabel: {
    minWidth: '70px',
  },
};
