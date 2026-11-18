import { Link } from 'react-router-dom';
import roomImage from '../assets/rom.jpg';

const HomePage = () => {
  return (

    <div className="home-hero">


      <section className="home-intro">

        <h1 className="home-title">
          Et godt sted å tenke sammen.
        </h1>

        <p className="home-description">
          Finn et grupperom som passer for deg. Søk etter bygg,
          kapasitet og ledighet — alt på ett sted.
        </p>

        <Link to="/rooms" className="btn home-search-button">
          Finn ledige rom
        </Link>
      </section>

      <img
        src={roomImage}
        alt="Grupperom med plass til samarbeid"
        className="home-image"
      />
    </div>

  );

};

export default HomePage;