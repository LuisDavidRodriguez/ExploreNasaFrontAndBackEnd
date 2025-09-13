import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import BandSection from './BandSection';

jest.mock('../ui/Swiper/Swiper', () => {
  return () => (<div data-testid="swiper">Swiper</div>)
});

describe('Render BandSection', () => {
  it('render basic', () => {
    render(<BandSection cards={[]} title="title-test" info="info-test" />);
    expect(screen.getByText('title-test'));
    expect(screen.getByText('info-test'));
    expect(screen.getByText('Swiper'));
  })
});