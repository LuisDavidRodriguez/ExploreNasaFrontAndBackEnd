import { configureStore } from '@reduxjs/toolkit';
import { allReducer, randomReducer } from './apod';

const store = configureStore({
  reducer: {
    randomApod: randomReducer,
    allApods: allReducer,
  },
});

export default store;
