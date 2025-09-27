import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Provider } from 'react-redux';
import { BrowserRouter as Router } from 'react-router-dom';
import { configureStore } from '@reduxjs/toolkit';
import ApodSearchSection from './ApodSearchSection';

// Mock the API and helper functions
jest.mock('../../../redux/apodApi', () => ({
  fetchDateApod: () => () => ({
    type: 'date_apod_fetched/pending',
    payload: undefined,
  })
}));

jest.mock('../../../helpers/cardsCreators', () => ({
  createCardsApod: jest.fn(() => [])
}));

// Mock child components
jest.mock('../FormFilter/FormFilter', () => {
  return function MockFormFilter({ reportInputs, buttonHandler1, buttonText1 }) {
    return (
      <div data-testid="form-filter">
        <button onClick={() => reportInputs({ text: 'test', date: '', mediaType: '' })}>
          Test Text Filter
        </button>
        <button onClick={() => reportInputs({ text: '', date: '2023-12-01', mediaType: '' })}>
          Test Date Filter
        </button>
        <button onClick={() => reportInputs({ text: '', date: '', mediaType: 'image' })}>
          Test Media Filter
        </button>
        <button onClick={buttonHandler1}>
          {buttonText1}
        </button>
      </div>
    );
  };
});

jest.mock('../../ui/SwiperGrid/SwiperGrid', () => {
  return function MockSwiperGrid({ cards }) {
    return <div data-testid="swiper-grid">{cards?.length ?? 0} cards</div>;
  };
});

jest.mock('../../ui/CardsImg/CardsImg', () => {
  return function MockCardsImg({ data, onClick }) {
    return (
      <div data-testid={`card-${data.id}`} onClick={() => onClick(data.id)}>
        {data.title}
      </div>
    );
  };
});

jest.mock('../../Details/DetailsApod', () => {
  return function MockDetailsApod({ data }) {
    return <div data-testid="details-apod">{data.title}</div>;
  };
});

jest.mock('../../ui/Modal/Modal', () => {
  return function MockModal({ children, show, handleClose }) {
    if (!show) return null;
    return (
      <div data-testid="modal">
        <button onClick={handleClose}>Close</button>
        {children}
      </div>
    );
  };
});

// Helper function to create mock store
const createMockStore = (initialState = {}) => {
  const defaultState = {
    allApods: {
      filters: {
        show: 'ALL',
        text: '',
        date: '',
        mediaType: '',
      },
      data: [
        {
          id: '1',
          title: 'Test APOD 1',
          explanation: 'This is a test explanation for APOD 1',
          date: '2023-12-01',
          mediaType: 'image',
          url: 'https://example.com/image1.jpg'
        },
        {
          id: '2',
          title: 'Test APOD 2',
          explanation: 'This is a test explanation for APOD 2',
          date: '2023-12-02',
          mediaType: 'video',
          url: 'https://example.com/video1.mp4'
        }
      ],
      status: 'fulfilled'
    },
    ...initialState
  };

  return configureStore({
    reducer: {
      allApods: (state = defaultState.allApods) => state
    },
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware({
        serializableCheck: false,
        immutableCheck: false
      })
  });
};

// Helper function to render component with store
const renderWithStore = (store) => {
  return render(
    <Provider store={store}>
      <Router>
        <ApodSearchSection />
      </Router>
    </Provider>
  );
};

describe('ApodSearchSection Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe('Rendering', () => {
    it('renders the main section with title', () => {
      const store = createMockStore();
      renderWithStore(store);
      expect(screen.getByText('Search something')).toBeInTheDocument();
    });

    it('renders FormFilter component', () => {
      const store = createMockStore();
      renderWithStore(store);

      expect(screen.getByTestId('form-filter')).toBeInTheDocument();
    });

  });

  describe('Data Loading', () => {
    it('dispatches fetchDateApod when status is empty', () => {
      const store = createMockStore({
        allApods: {
          filters: { show: 'ALL', text: '', date: '', mediaType: '' },
          data: [],
          status: 'empty'
        }
      });

      const dispatchSpy = jest.spyOn(store, 'dispatch');
      renderWithStore(store);

      // The component should attempt to dispatch fetchDateApod
      expect(dispatchSpy).toHaveBeenCalled();
    });

    it('does not dispatch fetchDateApod when status is not empty', () => {
      const store = createMockStore({
        allApods: {
          filters: { show: 'ALL', text: '', date: '', mediaType: '' },
          data: [],
          status: 'fulfilled'
        }
      });

      const dispatchSpy = jest.spyOn(store, 'dispatch');
      renderWithStore(store);

      expect(dispatchSpy).not.toHaveBeenCalled();
    });
  });

});