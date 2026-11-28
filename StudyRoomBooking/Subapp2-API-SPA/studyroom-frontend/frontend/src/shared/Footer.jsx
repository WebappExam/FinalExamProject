import footerLogo from '../assets/logo-footer.svg';
import facebookIcon from '../assets/icon-facebook.svg';
import twitterIcon from '../assets/icon-twitter.svg';
import linkedinIcon from '../assets/icon-linkedin.svg';
import instagramIcon from '../assets/icon-instagram.svg';

const Footer = () => {
    return (
        <footer className="bottom-footer">
            <section className="nyhetsbrev-container">
                <div className="nyhetsbrev">
                    <h3>Hold deg oppdatert på hva som skjer på OsloMet.</h3>
                </div>

                <a href="https://www.oslomet.no/nyhetsbrev">
                    <div className="nyhetsbrev-button">
                        <h3>Abonner på vårt nyhetsbrev!</h3>
                    </div>
                </a>
            </section>
            <section className="flex-container">
                <div className="top-footer-border"></div>
            </section>

            <section className="flex-container">
                <div className="about">
                    <h3>
                        <img src={footerLogo} alt="OsloMet – storbyuniversitetet" />
                    </h3>
                    <ul>
                        <li>Postboks 4, St. Olavs plass</li>
                        <li>0130 Oslo</li>
                        <li>Tlf.: 67 23 50 00</li>
                        <li className="link-kontakt-oss">
                            <a href="https://www.oslomet.no/om/kontakt">Kontakt oss</a>
                        </li>
                    </ul>
                </div>

                <div class="related-links">
                    <h3>Aktuelle linker</h3>
                    <ul>
                        <li><a href="https://www.oslomet.no/alumni">Alumni</a></li>
                        <li><a href="https://www.oslomet.no/om/ansatteoversikt">Ansatteoversikt</a></li>
                        <li><a href="https://www.oslomet.no/om/arrangement/arrangementsoversikt">Arrangementsoversikt</a></li>
                        <li><a href="https://www.oslomet.no/om/for-ansatte">For ansatte</a></li>
                        <li><a href="https://www.oslomet.no/om/for-studenter">For studenter</a></li>
                        <li><a href="https://www.oslomet.no/om/ledige-stillinger">Ledige stillinger</a></li>
                        <li><a href="https://www.oslomet.no/om/personvernerklering-informasjonskapslar">Personvernerklæring og informasjonskapslar</a></li>
                        <li><a href="https://www.oslomet.no/om/pressekontakt">Pressekontakt</a></li>
                        <li><a href="https://www.oslomet.no/om/tilgjengelegheitserklering">Tilgjengelegheitserklæring</a></li>
                        <li><a href="https://login.microsoftonline.com/fec81f12-6286-4550-8911-f446fcdafa1f/saml2?SAMLRequest=fVLLjtMwFP2VyHs3ifNoY7WVylSISgNU08KCDXKc66klP4KvQ%2BHvSZNBlE1Xlo7P496ju0ZhTc93Q7y4F%2FgxAMbklzUO%2BfSxIUNw3AvUyJ2wgDxKftp9fOZskfE%2B%2BOilN%2BRO8lghECFE7R1JDvsN%2Bd6yOmtFUzdNUXaNbKuy6Io2g6phRbFaLps8Y7KqOlmQ5CsEHJUbMhqNcsQBDg6jcHGEMlbSPKcsO%2BcZL5a8Kr%2BRZD9uo52Ik%2BoSY488TY1%2F1W5htQwevYreGe1gIb1NFchVrnJGa7aqaVlVGV01o6kqy1rJTiiRq%2FS2IyPJ7u8iT97hYCGcIPzUEr68PP%2BLul6vVHR2jPNovIW4cH42UCK1vhsMLPpLP0Epzi%2BjQuKEdqDEYCLFniTHt6bfaddp9%2Fq45HYmIf9wPh%2Fp8fPpTLbrmzefSgvbu%2FnuJ1un96T1fBmfRvvD%2FuiNlr%2BT9z5YER%2Bn3xDdUTVReQzCoQYXx8qM8denACLChsQwAEm3c%2BT%2F97f9Aw%3D%3D&RelayState=https%3A%2F%2Fwww-admin.oslomet.no%2Fadmin%2Fview%2Fcontent%2F58148">Admin</a></li>
                    </ul>
                </div>

                <div class="socials">
                    <h3>Sosiale medier</h3>
                    <ul>
                        <li>
                            <a href="https://www.facebook.com/oslomet/">
                                <img src={facebookIcon} alt="" /> Facebook
                            </a>
                        </li>
                        <li>
                            <a href="https://x.com/i/flow/login?redirect_after_login=%2Foslomet">
                              <img src={twitterIcon} alt="" /> X (Twitter)
                            </a>
                        </li>
                        <li>
                            <a href="https://no.linkedin.com/school/oslomet/">
                                <img src={linkedinIcon} alt="" /> LinkedIn
                            </a>
                        </li>
                        <li>
                            <a href="https://www.instagram.com/oslomet/?hl=nb">
                                <img src={instagramIcon} alt="" /> Instagram
                            </a>
                        </li>
                    </ul>
                </div>
            </section>
        </footer>
    );
};

export default Footer;