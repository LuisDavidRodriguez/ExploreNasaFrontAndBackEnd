// I created this helper because it is important to know
// that the information received from each API is pretty different
// so it is better deal with each information in only this place
// instead of has in other part of the program again how to clean the data

const createCardsApod = (data, Cards, clickMore, cardFamily) => {
  let cards = [];
  if (data === 'loading') {
    // colocamos fakes id for the skeleton
    cards = [1845, 2848, 3545].map((skeleton) => (
      <Cards
        key={skeleton}
        id={skeleton}
      />
    ));
  } else {
    cards = data.map((apod) => (
      <Cards
        key={apod.id}
        id={apod.id}
        title={apod.title}
        topic={apod.date}
        information={apod.explanation}
        url={apod.url}
        altPicture={apod.title}
        mediaType={apod.mediaType}
        clickMore={clickMore}
        cardFamily={cardFamily}
      />
    ));
  }

  return cards;
};

export { createCardsApod };
