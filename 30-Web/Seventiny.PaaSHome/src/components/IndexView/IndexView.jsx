import React, { Component } from 'react';
import IceContainer from '@icedesign/container';
import './IndexView.scss';
import TableList from '../TableList';
import SearchForm from '../SearchForm';

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
      metaObjectCode: 'Student'
    }
  }

  conditionChange = (value) => {
    alert(JSON.stringify(value))
  };

  render() {
    return (
      <IceContainer style={styles.container}>
        <SearchForm onChange={this.conditionChange} />
        <TableList metaObject={this.state.metaObjectCode} />
      </IceContainer>
    );
  }
}

