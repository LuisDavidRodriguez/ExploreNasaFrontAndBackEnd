import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import FormFilter from './FormFilter';

// Mock props for testing
const mockReportInputs = jest.fn();
const mockButtonHandler1 = jest.fn();

const defaultProps = {
  reportInputs: mockReportInputs,
  buttonHandler1: mockButtonHandler1,
  buttonText1: 'Clear Filters'
};

const getTextInput = () => screen.getByPlaceholderText('search by text in this period');
const getDateInput = () => screen.getByLabelText('Select date');
const getSelectInput = () => screen.getByLabelText('Select media type');
const getClearButton = () => screen.getByRole('button', { name: 'Clear Filters' });

describe('FormFilter Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });


  describe('Rendering', () => {
    it('renders all form elements correctly', () => {
      render(<FormFilter {...defaultProps} />);
    
      expect(getTextInput()).toBeInTheDocument();
      expect(getDateInput()).toBeInTheDocument();
      expect(getSelectInput()).toBeInTheDocument();
      expect(getClearButton()).toBeInTheDocument();
    });


    it('renders select options correctly', () => {
      render(<FormFilter {...defaultProps} />);
      
      const select = screen.getByLabelText('Select media type');


      expect(select).toHaveValue('All');
      
      const options = screen.getAllByRole('option');
      expect(options).toHaveLength(3);
      expect(screen.getByRole('option', { name: 'All' })).toBeInTheDocument();
      expect(screen.getByRole('option', { name: 'Image' })).toBeInTheDocument();
      expect(screen.getByRole('option', { name: 'Video' })).toBeInTheDocument();
    });
  });

  describe('User Interactions', () => {
    it('updates text input and calls reportInputs with correct data', () => {
      render(<FormFilter {...defaultProps} />);
      
      const textInput = getTextInput();
      fireEvent.change(textInput, { target: { value: 'test search' } });
      
      expect(textInput).toHaveValue('test search');
      expect(mockReportInputs).toHaveBeenCalledWith({
        text: 'test search',
        date: '',
        mediaType: ''
      });
    });

    it('updates date input and calls reportInputs with correct data', () => {
      render(<FormFilter {...defaultProps} />);
      
      const dateInput = getDateInput();
      fireEvent.change(dateInput, { target: { value: '2023-12-01' } });
      
      expect(dateInput).toHaveValue('2023-12-01');
      expect(mockReportInputs).toHaveBeenCalledWith({
        date: '2023-12-01',
        text: '',
        mediaType: ''
      });
    });

    it('updates select input and calls reportInputs with correct data', () => {
      render(<FormFilter {...defaultProps} />);
      
      const selectInput = getSelectInput();
      fireEvent.change(selectInput, { target: { value: 'image' } });
      
      expect(selectInput).toHaveValue('image');
      expect(mockReportInputs).toHaveBeenCalledWith({
        text: '',
        date: '',
        mediaType: 'image'
      });
    });

    it('handles video selection correctly', () => {
      render(<FormFilter {...defaultProps} />);
      
      const selectInput = getSelectInput();
      fireEvent.change(selectInput, { target: { value: 'video' } });
      
      expect(selectInput).toHaveValue('video');
      expect(mockReportInputs).toHaveBeenCalledWith({
        text: '',
        date: '',
        mediaType: 'video'
      });
    });
  });
});
