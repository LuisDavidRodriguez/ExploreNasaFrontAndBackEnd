import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DetailsApod from './DetailsApod';

describe('DetailsApod', () => {
  it('Should render', () => {
    render(<DetailsApod data={{ title: "testTitle", url: "testUrl", mediaType: "image" }}/>)
    screen.debug(undefined, 10000);

    const image = screen.getByRole('img');
    expect(screen.getByText('testTitle'));
    expect(image).toBeInTheDocument;
    expect(image).toHaveAttribute('alt', 'testTitle');
    expect(image).toHaveAttribute('src', 'testUrl');
  })
});