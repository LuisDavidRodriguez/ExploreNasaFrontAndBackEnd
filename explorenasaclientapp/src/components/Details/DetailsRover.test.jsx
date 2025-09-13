import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DetailsRover from './DetailsRover';

describe('DetailsRover', () => {
  it('Should render', () => {
    render(<DetailsRover data={{
      id: 'testId',
      sol: 'testSol',
      earth_date: 'testEarth',
      camera: {
        name: 'testCamera',
        full_name: 'testCameraFullName',
      },
      rover: {
        name: 'testRoverName',
      },
      img_src: 'testSrc'
    }}/>)

    expect(screen.getByText(/testId/gi));
    expect(screen.getByText(/testSol/gi));
    expect(screen.getByText(/testEarth/gi));
    expect(screen.getByText(/testCamera -> testCameraFullName/gi));

    const image = screen.getByRole('img');
    expect(image).toBeInTheDocument;
    expect(image).toHaveAttribute('alt', 'testRoverName');
    expect(image).toHaveAttribute('src', 'testSrc');
  })
});