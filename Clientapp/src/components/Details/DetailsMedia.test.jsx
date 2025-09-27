import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DetailsMedia from './DetailsMedia';

describe('DetailsMedia', () => {
  it('Should render', () => {
    render(<DetailsMedia data={{ 
      data: [
        {
          center: 'testCenter',
          nasa_id: 'testId',
          title: 'testTitle',
          description: 'testDescription',
        },
      ],
      links: [
        {
          href: 'testHref',
          render: 'testRender'
        }
      ]
     }}/>)
    screen.debug(undefined, 10000);

    expect(screen.getByText(/testCenter/gi));
    expect(screen.getByText(/testId/gi));
    expect(screen.getByText(/testTitle/gi));
    expect(screen.getByText(/testDescription/gi));

    const image = screen.getByRole('img');
    expect(image).toBeInTheDocument;
    expect(image).toHaveAttribute('alt', 'testTitle');
    expect(image).toHaveAttribute('src', 'testHref');
  })
});